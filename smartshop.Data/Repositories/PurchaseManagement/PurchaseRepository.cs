using smartshop.Common.Constants;
using smartshop.Common.Enums.Accounting;
using smartshop.Common.QueryParams.Purchase;
using smartshop.Entities.Accounting;
using smartshop.Entities.PurchaseManagement;
using System.Transactions;

namespace smartshop.Data.Repositories
{
    internal class PurchaseRepository : IPurchaseRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IBankAccountRepsitory _bankAccountRepsitory;
        private readonly IHeadTransactionRepository _headTransactionRepository;

        public PurchaseRepository(ApplicationDbContext applicationDbContext, IBankAccountRepsitory bankAccountRepsitory, IHeadTransactionRepository headTransactionRepository)
        {
            this._dbContext = applicationDbContext;
            this._bankAccountRepsitory = bankAccountRepsitory;
            this._headTransactionRepository = headTransactionRepository;
        }
        public bool AnyDuePaymentOrReturn(int businessId, int id)
        {
            return Query(businessId)
                .Include(p => p.DuePayments)
                .Include(p => p.PurchaseReturns)
                .Where(x => x.Id == id)
                .Any(p => p.DuePayments.Count > 0 || p.PurchaseReturns.Count > 0);
        }

        public int Create(Purchase entity, int businessId)
        {
            using (TransactionScope ts = new TransactionScope())
            {
                _dbContext.Purchases.Add(entity);
                _dbContext.SaveChanges();

                AccountStatusChanges(entity);

                ts.Complete();
                return entity.Id;
            }

        }
        public void Delete(int id)
        {
            using (TransactionScope ts =  new TransactionScope())
            {
                var purchase = _dbContext.Purchases.Find(id);
                _dbContext.Purchases.Remove(purchase);
                _dbContext.SaveChanges();

                ReversePurchaseCalculation(purchase);
                ts.Complete();
            }

        }

        public bool Exists(int businessId, int id)
        {
            return Query(businessId).Any(p => p.Id == id);
        }

        public PaginationResponse<Purchase> Get(int businessId, int currentPage, int pageSize, PurchaseQueryParams searchParams)
        {
            var purchases = Filter(businessId, searchParams);

            return new PaginationResponse<Purchase>()
            {
                Rows = purchases.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList(),
                TotalRows = purchases.Count()
            };
        }

        public IEnumerable<Purchase> Get(int businessId, int? storeId, int? supplierId)
        {
            var query = _dbContext.Purchases
                .Include(p => p.Store)
                .Include(p => p.Supplier)
                .ThenInclude(p => p.Head)
                .Include(p => p.PurchaseProducts)
                .Include(p => p.DuePayments)
                .Where(p => p.Store.BusinessId == businessId)
                //.Where(p => p.GetCurrentDue() > 0)
                .AsQueryable();

            if(storeId != null)
                query = query.Where(p=>p.StoreId == storeId);

             if(supplierId != null)
                query = query.Where(p => p.SupplierId == supplierId);

            return query.ToList();
        }

        public IEnumerable<Purchase> Get(int businessId, PurchaseQueryParams searchParams)
        {
            return Filter(businessId, searchParams).ToList();
        }

        public Purchase? Get(int businessId, int id)
        {
            return Query(businessId)
                .Include(x => x.PurchaseProducts)
                .ThenInclude(p => p.Product)
                .ThenInclude(p => p.Head)
                .Include(x => x.PurchaseProducts)
                .ThenInclude(x => x.UnitVariation)
                .ThenInclude(x => x.Unit)
                .Include(x => x.PurchaseProducts)
                .ThenInclude(p => p.Product)
                .ThenInclude(p => p.Brand)
                .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<PurchaseProduct> GetPurchaseProducts(int businessId, int id)
        {
            return _dbContext.PurchaseProducts
                .Include(p => p.Purchase)
                .ThenInclude(p => p.Store)
                .Include(p => p.UnitVariation)
                .Where(pp => pp.PurchaseId == id && pp.Purchase.Store.BusinessId == businessId)
                .ToList();
        }

        public void Update(int id, Purchase entity)
        {
            using (var ts  = new TransactionScope())
            {
                var purchase = Query().FirstOrDefault(p => p.Id == id);
                
                ReversePurchaseCalculation(purchase);

                _dbContext.PurchaseProducts.RemoveRange(purchase.PurchaseProducts);

                purchase.BankAccountId = entity.BankAccountId;
                purchase.SupplierId = entity.SupplierId;
                purchase.StoreId = entity.StoreId;
                purchase.Date = entity.Date;
                purchase.Discount = entity.Discount;
                purchase.DiscountType = entity.DiscountType;
                purchase.Overhead = entity.Overhead;
                purchase.Paid = entity.Paid;

                _dbContext.PurchaseProducts.AddRange(entity.PurchaseProducts);

                _dbContext.Entry(purchase).State = EntityState.Modified;
                _dbContext.SaveChanges();

                AccountStatusChanges(purchase);

                ts.Complete();
            }
        }

        private IQueryable<Purchase> Filter(int businessId, PurchaseQueryParams queryParams)
        {
            var query = Query(businessId);

            if (queryParams.SupplierId != null)
                query = query.Where(p => p.SupplierId == queryParams.SupplierId);
            
            if (queryParams.StoreId != null)
                query = query.Where(p => p.StoreId == queryParams.StoreId);
            
            if (queryParams.InvoiceNumber != null)
                query = query.Where(p => p.InvoiceNumber.Contains(queryParams.InvoiceNumber)); 

            if (queryParams.Date != null)
                query = query.Where(p => p.Date.Date == queryParams.Date.Value.Date);


            return query;
        }

        private IQueryable<Purchase> Query(int? businessId = null)
        {
            var query = _dbContext.Purchases
                 .Include(x => x.Supplier)
                 .ThenInclude(s => s.Head)
                 .Include(x => x.Store)
                 .Include(x => x.BankAccount)
                 .ThenInclude(x => x.Head)
                 .AsQueryable();

            if (businessId != null)
                query = query.Where(x => x.Store.BusinessId == businessId);

            return query.OrderByDescending(x => x.Id);
        }

        /// <summary>
        ///  Balance account when purchase delete or Edit
        /// </summary>
        /// <param name="purchase"></param>
        private void ReversePurchaseCalculation(Purchase purchase)
        {
            //Update Bank Account
            var bankAccount = _bankAccountRepsitory.GetById(purchase.BankAccountId);
            bankAccount.Balance += purchase.Paid;
            _bankAccountRepsitory.Update(bankAccount);

            _headTransactionRepository.DeleteByPurchase(purchase.Id);
        }

        /// <summary>
        /// Account status changed when purchase create or update
        /// BankAccount Balance Calculate
        /// Head Transactions added
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="businessId"></param>
        private void AccountStatusChanges(Purchase entity)
        {
            //Update Bank Account
            var bankAccount = _bankAccountRepsitory.GetById(entity.BankAccountId);
            bankAccount.Balance -= entity.Paid;
            _bankAccountRepsitory.Update(bankAccount);

            //Add Heads
            var listOfHeadTransactions = new List<HeadTransaction>()
                {
                    new HeadTransaction(){
                        purchaseId = entity.Id,
                        Amount = entity.GetPayableAmount(),
                        Date = entity.Date,
                        Descriptions = $"Purchase bill for purchase invoice-{entity.InvoiceNumber}",
                        Type = TransactionType.Credit,
                        HeadId = entity.SupplierId,
                        AccountTransactions = new List<AccountTransaction>()
                        {
                            new AccountTransaction(){AccountId = TransactionAccount.PRODUCT, Operator = Operators.PLUS},
                            new AccountTransaction(){AccountId = TransactionAccount.ACCOUNT_PAYABLE, Operator = Operators.PLUS},
                        }
                    },

                    //Paid Transaction Add
                    new HeadTransaction(){
                        purchaseId = entity.Id,
                        Amount = entity.Paid,
                        Date = entity.Date,
                        Descriptions = $"Purchase paid for purchase invoice-{entity.InvoiceNumber}",
                        Type = TransactionType.Debit,
                        HeadId = entity.SupplierId,
                        AccountTransactions = new List<AccountTransaction>()
                        {
                            new AccountTransaction(){AccountId = TransactionAccount.CASH, Operator = Operators.MINUS},
                            new AccountTransaction(){AccountId = TransactionAccount.ACCOUNT_PAYABLE, Operator = Operators.MINUS},
                        }
                    },
                      new HeadTransaction()
                      {
                          purchaseId = entity.Id,
                          Amount= entity.Paid,
                          Date = entity.Date,
                          Descriptions = $"Purchase paid for purchase invoice-{entity.InvoiceNumber}",
                          Type = TransactionType.Debit,
                          HeadId = entity.BankAccountId,
                      }
                };

            _headTransactionRepository.Create(listOfHeadTransactions);
        }

    }
}
