using smartshop.Common.Constants;
using smartshop.Common.Enums.Accounting;
using smartshop.Common.QueryParams.Accountant;
using smartshop.Entities.Accounting;
using System.Transactions;

namespace smartshop.Data.Repositories
{
    internal class IncomeExpenseRepository : IIncomeExpenseRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IBankAccountRepsitory _bankAccountRepsitory;
        private readonly IHeadTransactionRepository _headTransactionRepository;

        public IncomeExpenseRepository(ApplicationDbContext dbContext, IBankAccountRepsitory bankAccountRepsitory, IHeadTransactionRepository headTransactionRepository)
        {
            this._dbContext = dbContext;
            this._bankAccountRepsitory = bankAccountRepsitory;
            this._headTransactionRepository = headTransactionRepository;
        }
        public int Create(IncomeExpense entity)
        {
            using (var ts = new TransactionScope())
            {
                _dbContext.IncomeExpenses.Add(entity);
                Save();

                //Calculate Accounting
                AccountStatusChanges(entity);
                ts.Complete();
                return entity.Id;
            }
        }

        public void Delete(int id)
        {
            using (var ts = new TransactionScope())
            {
                var entity = GetQuery().Single(b => b.Id == id);

                ReverseCalculation(entity);

                _dbContext.IncomeExpenses.Remove(entity);
                Save();
                ts.Complete();
            }
        }

        public bool Exists(int businessId, int id)
            => GetQuery(businessId).Any(x => x.Id == id);

        public PaginationResponse<IncomeExpense> Gets(int businessId, IncomeExpenseQueryParams paginateQueryParams)
        {
            var query = GetQuery(businessId);
            if (paginateQueryParams.HeadId != null)
                query = query.Where(dp => dp.HeadId == paginateQueryParams.HeadId);

            if (paginateQueryParams.Income != null)
            {
                var isIncome = paginateQueryParams.Income == 1;
                query = query.Where(i => i.Income == isIncome);
            }

            return new PaginationResponse<IncomeExpense>
            {
                Rows = query.Skip((paginateQueryParams.Page - 1) * paginateQueryParams.Size).Take(paginateQueryParams.Size),
                TotalRows = query.Count()
            };
        }

        public IncomeExpense? Get(int businessId, int id)
            => GetQuery(businessId).FirstOrDefault(b => b.Id == id);


        public void Update(int id, IncomeExpense entity)
        {
            using (var ts = new TransactionScope())
            {
                var incomeExpense = GetQuery().SingleOrDefault(x => x.Id == id);

                ReverseCalculation(incomeExpense);

                incomeExpense.BankAccountId = entity.BankAccountId;
                incomeExpense.HeadId = entity.HeadId;
                incomeExpense.Amount = entity.Amount;
                incomeExpense.Date = entity.Date;
                incomeExpense.Description = entity.Description;

                _dbContext.Entry(incomeExpense).State = EntityState.Modified;
                Save();

                AccountStatusChanges(incomeExpense);

                ts.Complete();
            }
        }

        private IQueryable<IncomeExpense> GetQuery(int? businessId = null)
        {
            var query = _dbContext.IncomeExpenses
                .Include(s => s.Head)
                .Include(dp => dp.BankAccount)
                .ThenInclude(b => b.Head)
                .OrderByDescending(i => i.Id)
                .AsQueryable();

            if (businessId != null) query = query.Where(x => x.Head.BusinessId == businessId);

            return query;
        }

        private bool Save()
        {
            return _dbContext.SaveChanges() > 0;
        }


        /// <summary>
        ///  Balanced account when sale delete or Edit
        /// </summary>
        /// <param name="sale"></param>
        private void ReverseCalculation(IncomeExpense incomeExpense)
        {

            //Update Bank Account
            var bankAccount = _bankAccountRepsitory.GetById(incomeExpense.BankAccountId);
            if (incomeExpense.Income)
                bankAccount.Balance -= incomeExpense.Amount;
            else
                bankAccount.Balance += incomeExpense.Amount;
            _bankAccountRepsitory.Update(bankAccount);

            _headTransactionRepository.DeleteByIncomeExpense(incomeExpense.Id);
        }

        /// <summary>
        /// Account status changed when sale create or update
        /// BankAccount Balance Calculate
        /// Head Transactions added
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="businessId"></param>
        private void AccountStatusChanges(IncomeExpense incomeExpense)
        {
           
            //Update Bank Account
            var bankAccount = _bankAccountRepsitory.GetById(incomeExpense.BankAccountId);
            if(incomeExpense.Income)
                bankAccount.Balance += incomeExpense.Amount;
            else
                bankAccount.Balance -= incomeExpense.Amount;
            _bankAccountRepsitory.Update(bankAccount);

            //Add Heads
            var listOfHeadTransactions = new List<HeadTransaction>()
                {
                    //Paid Transaction Add
                    new HeadTransaction(){
                        DueCollectionId = incomeExpense.Id,
                        Amount = incomeExpense.Amount,
                        Date = incomeExpense.Date,
                        Descriptions = incomeExpense.Description,
                        Type = incomeExpense.Income ? TransactionType.Credit : TransactionType.Debit,
                        HeadId = incomeExpense.HeadId,
                        AccountTransactions = new List<AccountTransaction>()
                        {
                            new AccountTransaction(){AccountId = TransactionAccount.CASH, Operator = incomeExpense.Income ? Operators.PLUS : Operators.MINUS},
                            new AccountTransaction(){AccountId = incomeExpense.Income ? TransactionAccount.REVENIES : TransactionAccount.EXPENSE, Operator = incomeExpense.Income ? Operators.PLUS : Operators.MINUS},
                        }
                    },
                      new HeadTransaction()
                      {
                         DueCollectionId = incomeExpense.Id,
                          Amount= incomeExpense.Amount,
                          Date = incomeExpense.Date,
                          Descriptions = incomeExpense.Description,
                          Type = incomeExpense.Income ? TransactionType.Credit : TransactionType.Debit,
                          HeadId = incomeExpense.BankAccountId,
                      }
                };

            _headTransactionRepository.Create(listOfHeadTransactions);
        }
    }
}
