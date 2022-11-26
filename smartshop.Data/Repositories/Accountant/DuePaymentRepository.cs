using smartshop.Common.Constants;
using smartshop.Common.Enums.Accounting;
using smartshop.Data.IRepositories;
using smartshop.Entities.Accounting;
using System.Transactions;

namespace smartshop.Data.Repositories
{
    internal class DuePaymentRepository : IDuePaymentRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IBankAccountRepsitory _bankAccountRepsitory;
        private readonly IHeadTransactionRepository _headTransactionRepository;

        public DuePaymentRepository(ApplicationDbContext dbContext, IBankAccountRepsitory bankAccountRepsitory, IHeadTransactionRepository headTransactionRepository)
        {
            this._dbContext = dbContext;
            this._bankAccountRepsitory = bankAccountRepsitory;
            this._headTransactionRepository = headTransactionRepository;
        }
        public int Create(DuePayment entity)
        {
            using (var ts = new TransactionScope())
            {
                _dbContext.DuePayments.Add(entity);
                Save();

                AccountStatusChanges(entity.Id);

                ts.Complete();
                return entity.Id;
            }
        }

        public void Delete(int id)
        {
            using (var ts = new TransactionScope())
            {
                var entity = GetDuePayments().Single(b => b.Id == id);

                ReverseCalculation(entity);

                _dbContext.DuePayments.Remove(entity);
                Save();

                ts.Complete();
            }
        }

        public bool Exists(int businessId, int id)
            => GetDuePayments(businessId).Any(x => x.Id == id);

        public string GenerateSlip(int businessId)
        {
            var prefix = _dbContext.BusinessConfigures.FirstOrDefault(b => b.Id == businessId).DuePaymentPrefix;
            var last = _dbContext.DuePayments.OrderByDescending(x => x.Id).FirstOrDefault();
            if (last == null)
                return $"{prefix}-{DateTime.UtcNow.ToString("yyyy")}-{1}";

            return $"{prefix}-{DateTime.UtcNow.ToString("yyyy")}-{last.Id + 1}";
        }

        public PaginationResponse<DuePayment> Get(int businessId, DuePaymentQueryParams queryParams)
        {
            var query = GetDuePayments(businessId);
            if (queryParams.StoreId != null)
                query = query.Where(dp => dp.Purchase.StoreId == queryParams.StoreId.Value);

            if (queryParams.SupplierId != null)
                query = query.Where(dp => dp.Purchase.SupplierId == queryParams.SupplierId.Value);
            
            if (queryParams.Date != null)
                query = query.Where(dp => dp.Date.Date == queryParams.Date.Value.Date);

            if(!string.IsNullOrEmpty(queryParams.InvoiceNumber))
                query = query.Where(dp => dp.Purchase.InvoiceNumber.ToLower().Contains(queryParams.InvoiceNumber.ToLower()));

            return new PaginationResponse<DuePayment>
            {
                Rows = query.Skip((queryParams.Page - 1) * queryParams.Size).Take(queryParams.Size),
                TotalRows = query.Count()
            };
        }

        public DuePayment? Get(int businessId, int id)
            => GetDuePayments(businessId).FirstOrDefault(b => b.Id == id);

        public IEnumerable<DuePayment> Get(int businessId)
            => GetDuePayments(businessId).ToList();

        public int TotalCount(int businessId)
            => GetDuePayments(businessId).Count();

        public void Update(int id, DuePayment entity)
        {
            using (var ts = new TransactionScope())
            {
                var duePayment = GetDuePayments().SingleOrDefault(x => x.Id == id);

                ReverseCalculation(duePayment);

                duePayment.BankAccountId = entity.BankAccountId;
                duePayment.PreviousDue = entity.PreviousDue;
                duePayment.PurchaseId = entity.PurchaseId;
                duePayment.Amount = entity.Amount;
                duePayment.Discount = entity.Discount;
                duePayment.DiscountType = entity.DiscountType;
                duePayment.Date = entity.Date;

                _dbContext.Entry(duePayment).State = EntityState.Modified;
                Save();

                AccountStatusChanges(id);
                ts.Complete();
            }
        }

        private IQueryable<DuePayment> GetDuePayments(int? businessId = null)
        {
            var query = _dbContext.DuePayments
                .Include(s => s.Purchase)
                .ThenInclude(s => s.Store)
                .Include(dp => dp.Purchase.Supplier)
                .ThenInclude(dp => dp.Head)
                .Include(dp => dp.BankAccount)
                .ThenInclude(b => b.Head)
                .AsQueryable();

            if (businessId != null) query = query.Where(x => x.Purchase.Store.BusinessId == businessId);

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
        private void ReverseCalculation(DuePayment duePayment)
        {
            //Update Bank Account
            var bankAccount = _bankAccountRepsitory.GetById(duePayment.BankAccountId);
            bankAccount.Balance += duePayment.Amount - duePayment.GetDiscountAmount();
            _bankAccountRepsitory.Update(bankAccount);

            _headTransactionRepository.DeleteByDuePayment(duePayment.Id);
        }

        /// <summary>
        /// Account status changed when sale create or update
        /// BankAccount Balance Calculate
        /// Head Transactions added
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="businessId"></param>
        private void AccountStatusChanges(int duePaymentId)
        {
            var duePayment = GetDuePayments().FirstOrDefault(dc => dc.Id == duePaymentId);

            //Update Bank Account
            var bankAccount = _bankAccountRepsitory.GetById(duePayment.BankAccountId);
            bankAccount.Balance -= duePayment.Amount - duePayment.GetDiscountAmount();
            _bankAccountRepsitory.Update(bankAccount);

            //Add Heads
            var listOfHeadTransactions = new List<HeadTransaction>()
                {
                    //Paid Transaction Add
                    new HeadTransaction(){
                        DuePaymentId = duePaymentId,
                        Amount = duePayment.Amount,
                        Date = duePayment.Date,
                        Descriptions = $"Due payment for slip no-{duePayment.PaymentSlipNumber}",
                        Type = TransactionType.Debit,
                        HeadId = duePayment.Purchase.SupplierId,
                        AccountTransactions = new List<AccountTransaction>()
                        {
                            new AccountTransaction(){AccountId = TransactionAccount.CASH, Operator = Operators.MINUS},
                            new AccountTransaction(){AccountId = TransactionAccount.ACCOUNT_PAYABLE, Operator = Operators.MINUS},
                        }
                    },
                      new HeadTransaction()
                      {
                         DuePaymentId = duePaymentId,
                          Amount= duePayment.Amount - duePayment.GetDiscountAmount(),
                          Date = duePayment.Date,
                          Descriptions = $"Due collection for slip no-{duePayment.PaymentSlipNumber}",
                          Type = TransactionType.Debit,
                          HeadId = duePayment.BankAccountId,
                      }
                };

            _headTransactionRepository.Create(listOfHeadTransactions);
        }
    }
}
