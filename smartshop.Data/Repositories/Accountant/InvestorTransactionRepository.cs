using smartshop.Common.Constants;
using smartshop.Common.Enums.Accounting;
using smartshop.Entities.Accounting;
using System.Transactions;

namespace smartshop.Data.Repositories
{
    internal class InvestorTransactionRepository : IInvestorTransactionRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IBankAccountRepsitory _bankAccountRepsitory;
        private readonly IHeadTransactionRepository _headTransactionRepository;

        public InvestorTransactionRepository(ApplicationDbContext applicationDbContext, IBankAccountRepsitory bankAccountRepsitory, IHeadTransactionRepository headTransactionRepository)
        {
            _applicationDbContext = applicationDbContext;
            this._bankAccountRepsitory = bankAccountRepsitory;
            this._headTransactionRepository = headTransactionRepository;
        }
        public int Create(InvestorTransaction entity)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                var tran = _applicationDbContext.InvestorTransactions.Add(entity);
                _applicationDbContext.SaveChanges();

                ChangeAccountStatus(entity.Id);

                ts.Complete();
                return entity.Id;
            }
        }


        public void Delete(int id)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                var transaction = _applicationDbContext.InvestorTransactions.Include(ip => ip.BankAccount).FirstOrDefault(x => x.Id == id);

                ReverseBalance(transaction);

                //Delete Transaction
                _applicationDbContext.InvestorTransactions.Remove(transaction);
                _applicationDbContext.SaveChanges();
                ts.Complete();
            }
        }


        public bool Exists(int businessId, int id)
        {
            return _applicationDbContext.InvestorTransactions
                .Include(t => t.Investor)
                .ThenInclude(i => i.Head)
                .Any(t => t.Id == id && t.Investor.Head.BusinessId == businessId);
        }

        public PaginationResponse<InvestorTransaction> Get(int businessId, InvestorTransactionQueryParams queryParams)
        {
            var query = CommonQuery(businessId);

            if (queryParams.InvestorId != null)
                query = query.Where(ip => ip.InvestorId == queryParams.InvestorId.Value);

            if (queryParams.Date != null)
                query = query.Where(ip => ip.Date.Date == queryParams.Date.Value.Date);
            
            if (queryParams.TransactionType != null)
                query = query.Where(ip => ip.TransactionType == queryParams.TransactionType.Value);

            return new PaginationResponse<InvestorTransaction>
            {
                Rows = query.Skip((queryParams.Page - 1) * queryParams.Size).Take(queryParams.Size),
                TotalRows = query.Count()
            };
        }

        public InvestorTransaction? Get(int businessId, int id) 
            => CommonQuery(businessId).FirstOrDefault(x => x.Id == id);

        public void Update(int id, InvestorTransaction entity)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                var transaction = _applicationDbContext.InvestorTransactions.Include(ip => ip.BankAccount).FirstOrDefault(x => x.Id == id);

                ReverseBalance(transaction);

                transaction.InvestorId = entity.InvestorId;
                transaction.BankAccountId = entity.BankAccountId;
                transaction.Amount = entity.Amount;
                transaction.Date = entity.Date;
                transaction.Descriptions = entity.Descriptions;

                _applicationDbContext.Entry(transaction).State = EntityState.Modified;
                _applicationDbContext.SaveChanges();

                ChangeAccountStatus(id);

                ts.Complete(); 
            }
        }

        private IQueryable<InvestorTransaction> CommonQuery(int? businessId = null)
        {
            var query = _applicationDbContext.InvestorTransactions
                .Include(ip => ip.Investor)
                .ThenInclude(i => i.Head)
                .Include(ip => ip.BankAccount)
                .ThenInclude(b => b.Head)
                .AsQueryable();
            if (businessId != null)
                query = query.Where(x => x.Investor.Head.BusinessId == businessId);
            return query;
        }
        private void ReverseBalance(InvestorTransaction? transaction)
        {
            //Update Bank Account
            var bankAccount = _bankAccountRepsitory.GetById(transaction.BankAccountId);
            if (transaction.TransactionType == TransactionType.Credit)
                bankAccount.Balance -= transaction.Amount;
            else
                bankAccount.Balance += transaction.Amount;
            _bankAccountRepsitory.Update(bankAccount);

            _headTransactionRepository.DeleteByInvestorTransaction(transaction.Id);
        }

        private void ChangeAccountStatus(int id)
        {
            var investorTransaction = CommonQuery().FirstOrDefault(dc => dc.Id == id);
            //Update Bank Account

            var bankAccount = _bankAccountRepsitory.GetById(investorTransaction.BankAccountId);
            if (investorTransaction.TransactionType == TransactionType.Credit)
            {
                bankAccount.Balance += investorTransaction.Amount;
                _bankAccountRepsitory.Update(bankAccount);

                var listOfHeadTransactions = new List<HeadTransaction>()
                {
                    //Paid Transaction Add
                    new HeadTransaction(){
                        InvestorTransactionId = id,
                        Amount = investorTransaction.Amount,
                        Date = investorTransaction.Date,
                        Descriptions = $"Invest by-{investorTransaction.Investor.Head.Name}",
                        Type = TransactionType.Credit,
                        HeadId = investorTransaction.InvestorId,
                        AccountTransactions = new List<AccountTransaction>()
                        {
                            new AccountTransaction(){AccountId = TransactionAccount.CASH, Operator = Operators.PLUS},
                            new AccountTransaction(){AccountId = TransactionAccount.CAPITAL, Operator = Operators.PLUS},
                        }
                    },
                    new HeadTransaction()
                    {
                        InvestorTransactionId = id,
                        Amount= investorTransaction.Amount,
                        Date = investorTransaction.Date,
                        Descriptions = $"Invest by-{investorTransaction.Investor.Head.Name}",
                        Type = TransactionType.Credit,
                        HeadId = investorTransaction.BankAccountId,
                    }

                };

                _headTransactionRepository.Create(listOfHeadTransactions);

            }
            else
            {
                bankAccount.Balance -= investorTransaction.Amount;
                _bankAccountRepsitory.Update(bankAccount);
 
                var listOfHeadTransactions = new List<HeadTransaction>()
                {
                    //Paid Transaction Add
                    new HeadTransaction(){
                        InvestorTransactionId = id,
                        Amount = investorTransaction.Amount,
                        Date = investorTransaction.Date,
                        Descriptions = $"Withdraw by-{investorTransaction.Investor.Head.Name}",
                        Type = TransactionType.Debit,
                        HeadId = investorTransaction.InvestorId,
                        AccountTransactions = new List<AccountTransaction>()
                        {
                            new AccountTransaction(){AccountId = TransactionAccount.CASH, Operator = Operators.MINUS},
                            new AccountTransaction(){AccountId = TransactionAccount.DRAWING, Operator = Operators.MINUS},
                        }
                    },
                    new HeadTransaction()
                    {
                        InvestorTransactionId = id,
                        Amount= investorTransaction.Amount,
                        Date = investorTransaction.Date,
                        Descriptions = $"Withdraw by-{investorTransaction.Investor.Head.Name}",
                        Type = TransactionType.Debit,
                        HeadId = investorTransaction.BankAccountId,
                    }
                };

                _headTransactionRepository.Create(listOfHeadTransactions);
            }
        }
    }
}
