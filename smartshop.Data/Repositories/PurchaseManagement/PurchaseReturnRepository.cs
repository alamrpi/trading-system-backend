using smartshop.Common.Constants;
using smartshop.Common.Enums.Accounting;
using smartshop.Common.QueryParams.Purchase;
using smartshop.Entities.Accounting;
using System.Transactions;

namespace smartshop.Data.Repositories
{
    internal class PurchaseReturnRepository : IPurchaseReturnRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHeadTransactionRepository _headTransactionRepository;

        public PurchaseReturnRepository(ApplicationDbContext dbContext, IHeadTransactionRepository headTransactionRepository)
        {
            _dbContext = dbContext;
            this._headTransactionRepository = headTransactionRepository;
        }
        public int Create(PurchaseReturn entity)
        {
            using (var ts = new TransactionScope())
            {
                _dbContext.PurchaseReturns.Add(entity);
                _dbContext.SaveChanges();

                //Head Transaction operations
                AccountStatusChanges(entity.Id);
                return entity.Id;
            }

        }

        public void Delete(int id)
        {
            var purchaseReturn = _dbContext.PurchaseReturns.Find(id);

            _dbContext.PurchaseReturns.Remove(purchaseReturn);
            _dbContext.SaveChanges();

            ReversePurchaseReturnCalculation(purchaseReturn.Id);
        }

        public bool Exists(int businessId, int id) 
            => _dbContext.PurchaseReturns
            .Include(pr => pr.Purchase)
            .ThenInclude(p => p.Store)
            .Any(x => x.Purchase.Store.BusinessId == businessId && x.Id == id);

        public PaginationResponse<PurchaseReturn> Get(int businessId, int currentPage, int pageSize, PurchaseQueryParams searchParams)
        {
            var purchases = Filter(businessId, searchParams);

            return new PaginationResponse<PurchaseReturn>()
            {
                Rows = purchases.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList(),
                TotalRows = purchases.Count()
            };
        }

        public IEnumerable<PurchaseReturn> Get(int businessId, PurchaseQueryParams searchParams)
        {
            return Filter(businessId, searchParams).ToList();
        }

        public PurchaseReturn? Get(int businessId, int id)
        {
            return Query(businessId)
                .Include(p => p.PurchaseReturnProducts)
                .ThenInclude(p => p.PurchaseProduct.Product)
                .ThenInclude(p => p.Head)
                .Include(p => p.PurchaseReturnProducts)
                .ThenInclude(p => p.PurchaseProduct.UnitVariation)
                .FirstOrDefault(x => x.Id == id);
        }

        public void Update(int id, PurchaseReturn entity)
        {
            var purchaseReturn = _dbContext.PurchaseReturns.FirstOrDefault(x => x.Id == id);
            if (purchaseReturn == null)
                throw new Exception("Purchase return not found!");

            //Reverser Calculations
            ReversePurchaseReturnCalculation(id);

            _dbContext.PurchaseReturnProducts.RemoveRange(purchaseReturn.PurchaseReturnProducts);

            purchaseReturn.Date = entity.Date;

            _dbContext.Entry(purchaseReturn).State = EntityState.Modified;

            _dbContext.PurchaseReturnProducts.AddRange(entity.PurchaseReturnProducts);

            _dbContext.SaveChanges();

            //Calculate Accounting
            AccountStatusChanges(entity.Id);
        }

        private IQueryable<PurchaseReturn> Filter(int businessId, PurchaseQueryParams queryParams)
        {
            var query = Query(businessId);

            if (queryParams.SupplierId != null)
                query = query.Where(p => p.Purchase.SupplierId == queryParams.SupplierId);

            if (queryParams.StoreId != null)
                query = query.Where(p => p.Purchase.StoreId == queryParams.StoreId);

            if (queryParams.InvoiceNumber != null)
                query = query.Where(p => p.InvoiceNumber.Contains(queryParams.InvoiceNumber));

            if (queryParams.Date != null)
                query = query.Where(p => p.Date.Date == queryParams.Date.Value.Date);


            return query;
        }

        private IQueryable<PurchaseReturn> Query(int? businessId = null)
        {
            var query = _dbContext.PurchaseReturns
                 .Include(x => x.PurchaseReturnProducts)
                 .ThenInclude(x => x.PurchaseProduct)
                 .Include(x => x.Purchase)
                 .ThenInclude(p => p.Supplier)
                 .ThenInclude(p => p.Head)
                 .Include(pr => pr.Purchase.Store)
                 .AsQueryable();

            if (businessId != null)
                query = query.Where(x => x.Purchase.Store.BusinessId == businessId);

            return query.OrderByDescending(x => x.Id);
        }

        private void AccountStatusChanges(int purchaseReturnId)
        {

            var purchaseReturn = Query().FirstOrDefault(x => x.Id == purchaseReturnId);
            //Add Heads
            var netAmount = purchaseReturn.PurchaseReturnProducts.Select(x => x.GetNetAmount()).Sum();
            var listOfHeadTransactions = new List<HeadTransaction>()
                {
                    new HeadTransaction(){
                        purchaseReturnId = purchaseReturnId,
                        Amount = netAmount,
                        Date = purchaseReturn.Date,
                        Descriptions = $"Purchase return bill for invoice-{purchaseReturn.InvoiceNumber}",
                        Type = TransactionType.Credit,
                        HeadId = purchaseReturn.Purchase.SupplierId,
                        AccountTransactions = new List<AccountTransaction>()
                        {
                            new AccountTransaction(){AccountId = TransactionAccount.PRODUCT, Operator = Operators.MINUS},
                            new AccountTransaction(){AccountId = TransactionAccount.ACCOUNT_PAYABLE, Operator = Operators.MINUS},
                        }
                    }
                };

            _headTransactionRepository.Create(listOfHeadTransactions);
        }

        private void ReversePurchaseReturnCalculation(int purchaseReturnId)
        {
            _headTransactionRepository.DeleteByPurchaseReturn(purchaseReturnId);
        }
    }
}
