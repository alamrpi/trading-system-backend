using smartshop.Common.Constants;
using smartshop.Common.Enums.Accounting;
using smartshop.Entities.Accounting;
using smartshop.Entities.SalesManagement;
using System.Transactions;

namespace smartshop.Data.Repositories
{
    internal class SalesReturnRepository : ISalesReturnRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHeadTransactionRepository _headTransactionRepository;

        public SalesReturnRepository(ApplicationDbContext dbContext, IHeadTransactionRepository headTransactionRepository)
        {
            _dbContext = dbContext;
            this._headTransactionRepository = headTransactionRepository;
        }
        public int Create(SaleReturn entity)
        {
            using (var ts = new TransactionScope())
            {
                _dbContext.SaleReturns.Add(entity);
                _dbContext.SaveChanges();

                AccountStatusChanges(entity.Id);
                return entity.Id;
            }
        }
        public void Delete(int id)
        {
            using (var ts = new TransactionScope())
            {

                var purchaseReturn = _dbContext.SaleReturns.Find(id);

                ReverseSaleReturnCalculation(id);

                _dbContext.SaleReturns.Remove(purchaseReturn);
                _dbContext.SaveChanges();
                ts.Complete();
            }
        }
        public bool Exists(int businessId, int id)
            => _dbContext.SaleReturns
            .Include(pr => pr.Sale)
            .ThenInclude(p => p.Store)
            .Any(x => x.Sale.Store.BusinessId == businessId && x.Id == id);

        public string GeneratSaleInvoiceNumber(int businessId)
        {
            var lastSale = _dbContext.SaleReturns.OrderByDescending(x => x.Id).FirstOrDefault();
            int lastId = 0;

            if (lastSale != null)
                lastId = lastSale.Id;

            return $"{DateTime.UtcNow:ddMMyyyy}-{lastId + 1}";
        }

        public PaginationResponse<SaleReturn> Get(int businessId, int currentPage, int pageSize, SalesQueryParams searchParams)
        {
            var sales = Filter(businessId, searchParams);

            return new PaginationResponse<SaleReturn>()
            {
                Rows = sales.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList(),
                TotalRows = sales.Count()
            };
        }

        public IEnumerable<SaleReturn> Get(int businessId, SalesQueryParams searchParams)
        {
            return Filter(businessId, searchParams).ToList();
        }

        public SaleReturn? Get(int businessId, int id)
        {
            return _dbContext.SaleReturns
                 .Include(x => x.Sale)
                 .ThenInclude(p => p.Customer)
                 .ThenInclude(p => p.Head)
                 .Include(pr => pr.Sale.Store)
                .FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<SaleReturnProduct> GetSaleReturnProducts(int id)
        {
            return _dbContext.SaleReturnProducts
                 .Include(sr => sr.SaleProduct)
                 .ThenInclude(sp => sp.Stock)
                 .ThenInclude(s => s.Product)
                 .ThenInclude(p => p.Head)
                 .Include(sr => sr.SaleProduct.UnitVariation)
                 .Include(sr => sr.SaleProduct.Stock.Product.Category)
                 .Include(sr => sr.SaleProduct.Stock.Product.Category.Group)
                 .Include(sr => sr.SaleProduct.Stock.Product.Brand)
                 .Where(x => x.SaleReturnId == id)
                 .ToList();
        }

        public void Update(int id, SaleReturn entity)
        {
            using (var ts = new TransactionScope())
            {
                var saleReturn = _dbContext.SaleReturns.FirstOrDefault(x => x.Id == id);
                if (saleReturn == null)
                    throw new Exception("Sale return not found!");

                ReverseSaleReturnCalculation(id);

                _dbContext.SaleReturnProducts.RemoveRange(saleReturn.SaleReturnProducts);

                saleReturn.Date = entity.Date;

                _dbContext.Entry(saleReturn).State = EntityState.Modified;

                _dbContext.SaleReturnProducts.AddRange(entity.SaleReturnProducts);

                AccountStatusChanges(id);

                _dbContext.SaveChanges();

                ts.Complete();
            }
        }

        private IQueryable<SaleReturn> Filter(int businessId, SalesQueryParams queryParams)
        {
            var query = Query(businessId);

            if (queryParams.CustomerId != null)
                query = query.Where(p => p.Sale.CustomerId == queryParams.CustomerId);

            if (queryParams.StoreId != null)
                query = query.Where(p => p.Sale.StoreId == queryParams.StoreId);

            if (queryParams.InvoiceNumber != null)
                query = query.Where(p => p.InvoiceNumber.Contains(queryParams.InvoiceNumber));

            if (queryParams.Date != null)
                query = query.Where(p => p.Date.Date == queryParams.Date.Value.Date); 
            
            if (queryParams.BankAccountId != null)
                query = query.Where(p => p.Sale.BankAccountId == queryParams.BankAccountId);

            return query;
        }

        private IQueryable<SaleReturn> Query(int? businessId = null)
        {
            var query = _dbContext.SaleReturns
                 .Include(x => x.SaleReturnProducts)
                 .ThenInclude(x => x.SaleProduct)
                 .Include(x => x.Sale)
                 .ThenInclude(p => p.Customer)
                 .ThenInclude(p => p.Head)
                 .Include(pr => pr.Sale.Store)
                 .AsQueryable();

            if (businessId != null)
                query = query.Where(x => x.Sale.Store.BusinessId == businessId);

            return query.OrderByDescending(x => x.Id);
        }

        private void AccountStatusChanges(int saleReturnId)
        {
            var saleReturns = Query().FirstOrDefault(x => x.Id == saleReturnId);
            //Add Heads
            var netAmount = saleReturns.SaleReturnProducts.Select(x => x.GetNetAmount()).Sum();
            var listOfHeadTransactions = new List<HeadTransaction>()
                {
                    new HeadTransaction(){
                        purchaseReturnId = saleReturnId,
                        Amount = netAmount,
                        Date = saleReturns.Date,
                        Descriptions = $"Sales return bill for invoice-{saleReturns.InvoiceNumber}",
                        Type = TransactionType.Credit,
                        HeadId = saleReturns.Sale.CustomerId,
                        AccountTransactions = new List<AccountTransaction>()
                        {
                            new AccountTransaction(){AccountId = TransactionAccount.PRODUCT, Operator = Operators.PLUS},
                            new AccountTransaction(){AccountId = TransactionAccount.ACCOUNT_RECEIVABLE, Operator = Operators.MINUS},
                        }
                    }
                };

            _headTransactionRepository.Create(listOfHeadTransactions);
        }

        private void ReverseSaleReturnCalculation(int id)
        {
            _headTransactionRepository.DeleteBySaleReturn(id);
        }
    }
}
