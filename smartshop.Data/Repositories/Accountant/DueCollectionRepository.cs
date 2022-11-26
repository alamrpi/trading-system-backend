using smartshop.Common.Constants;
using smartshop.Common.Enums.Accounting;
using smartshop.Data.IRepositories;
using smartshop.Entities.Accounting;
using smartshop.Entities.SalesManagement;
using System.Transactions;

namespace smartshop.Data.Repositories
{
    internal class DueCollectionRepository : IDueCollectionRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IBankAccountRepsitory _bankAccountRepsitory;
        private readonly IHeadTransactionRepository _headTransactionRepository;

        public DueCollectionRepository(ApplicationDbContext dbContext, IBankAccountRepsitory bankAccountRepsitory, IHeadTransactionRepository headTransactionRepository)
        {
            this._dbContext = dbContext;
            this._bankAccountRepsitory = bankAccountRepsitory;
            this._headTransactionRepository = headTransactionRepository;
        }
        public int Create(DueCollection entity)
        {
            using (var ts = new TransactionScope())
            {
                _dbContext.DueCollections.Add(entity);
                Save();

                //Calculate Accounting
                AccountStatusChanges(entity.Id);
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

                _dbContext.DueCollections.Remove(entity);
                ts.Complete();
                Save();
            }
        }

        public bool Exists(int businessId, int id)
            => GetQuery(businessId).Any(x => x.Id == id);

        public string GenerateSerialNo(int businessId)
        {
            var prefix = _dbContext.BusinessConfigures.FirstOrDefault(b => b.Id == businessId).DueCollectionPrefix;
            var last = _dbContext.DueCollections.OrderByDescending(x => x.Id).FirstOrDefault();
            if (last == null)
                return $"{prefix}-{DateTime.UtcNow.ToString("yyyy")}-{1}";

            return $"{prefix}-{DateTime.UtcNow.ToString("yyyy")}-{last.Id + 1}";
          
        }

        public PaginationResponse<DueCollection> Get(int businessId, DueCollectionQueryParams queryParams)
        {
            var query = GetQuery(businessId);
            if (queryParams.StoreId != null)
                query = query.Where(dp => dp.Sale.StoreId == queryParams.StoreId.Value);

            if (queryParams.CustomerId != null)
                query = query.Where(dp => dp.Sale.CustomerId == queryParams.CustomerId.Value);
            
            if (queryParams.Date != null)
                query = query.Where(dp => dp.Date.Date == queryParams.Date.Value.Date);

            if(!string.IsNullOrEmpty(queryParams.InvoiceNumber))
                query = query.Where(dp => dp.Sale.InvoiceNumber.ToLower().Contains(queryParams.InvoiceNumber.ToLower()));

            return new PaginationResponse<DueCollection>
            {
                Rows = query.Skip((queryParams.Page - 1) * queryParams.Size).Take(queryParams.Size),
                TotalRows = query.Count()
            };
        }

        public DueCollection? Get(int businessId, int id)
            => GetQuery(businessId).FirstOrDefault(b => b.Id == id);

        public IEnumerable<DueCollection> Get(int businessId)
            => GetQuery(businessId).ToList();

        public int TotalCount(int businessId)
            => GetQuery(businessId).Count();

        public void Update(int id, DueCollection entity)
        {
            using (var ts = new TransactionScope())
            {
                var dueCollection = GetQuery().SingleOrDefault(x => x.Id == id);

                ReverseCalculation(dueCollection);

                dueCollection.BankAccountId = entity.BankAccountId;
                dueCollection.SaleId = entity.SaleId;
                dueCollection.Amount = entity.Amount;
                dueCollection.Discount = entity.Discount;
                dueCollection.DiscountType = entity.DiscountType;
                dueCollection.Date = entity.Date;
                dueCollection.PreviouseDue = entity.PreviouseDue;

                _dbContext.Entry(dueCollection).State = EntityState.Modified;
                Save();

                AccountStatusChanges(id);

                ts.Complete();
            }
        }

        private IQueryable<DueCollection> GetQuery(int? businessId = null)
        {
            var query = _dbContext.DueCollections
                .Include(s => s.Sale)
                .ThenInclude(s => s.Store)
                .Include(dp => dp.Sale.Customer)
                .ThenInclude(dp => dp.Head)
                .Include(dp => dp.BankAccount)
                .ThenInclude(b => b.Head)
                .AsQueryable();

            if (businessId != null) query = query.Where(x => x.Sale.Store.BusinessId == businessId);

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
        private void ReverseCalculation(DueCollection dueCollection)
        {

            //Update Bank Account
            var bankAccount = _bankAccountRepsitory.GetById(dueCollection.BankAccountId);
            bankAccount.Balance -= dueCollection.Amount - dueCollection.GetDiscountAmount();
            _bankAccountRepsitory.Update(bankAccount);

            _headTransactionRepository.DeleteByDueCollection(dueCollection.Id);
        }

        /// <summary>
        /// Account status changed when sale create or update
        /// BankAccount Balance Calculate
        /// Head Transactions added
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="businessId"></param>
        private void AccountStatusChanges(int dueCollectionId)
        {
            var dueCollection = GetQuery().FirstOrDefault(dc => dc.Id == dueCollectionId);

            //Update Bank Account
            var bankAccount = _bankAccountRepsitory.GetById(dueCollection.BankAccountId);
            bankAccount.Balance += dueCollection.Amount - dueCollection.GetDiscountAmount();
            _bankAccountRepsitory.Update(bankAccount);

            //Add Heads
            var listOfHeadTransactions = new List<HeadTransaction>()
                {
                    //Paid Transaction Add
                    new HeadTransaction(){
                        DueCollectionId = dueCollectionId,
                        Amount = dueCollection.Amount,
                        Date = dueCollection.Date,
                        Descriptions = $"Due collection for slip no-{dueCollection.SlipNumber}",
                        Type = TransactionType.Credit,
                        HeadId = dueCollection.Sale.CustomerId,
                        AccountTransactions = new List<AccountTransaction>()
                        {
                            new AccountTransaction(){AccountId = TransactionAccount.CASH, Operator = Operators.PLUS},
                            new AccountTransaction(){AccountId = TransactionAccount.ACCOUNT_RECEIVABLE, Operator = Operators.MINUS},
                        }
                    },
                      new HeadTransaction()
                      {
                         DueCollectionId = dueCollectionId,
                          Amount= dueCollection.Amount - dueCollection.GetDiscountAmount(),
                          Date = dueCollection.Date,
                          Descriptions = $"Due collection for slip no-{dueCollection.SlipNumber}",
                          Type = TransactionType.Credit,
                          HeadId = dueCollection.BankAccountId,
                      }
                };

            _headTransactionRepository.Create(listOfHeadTransactions);
        }
    }
}
