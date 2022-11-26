using smartshop.Common.Constants;
using smartshop.Common.Enums.Accounting;
using smartshop.Data.IRepositories;
using smartshop.Entities;
using smartshop.Entities.Accounting;
using smartshop.Entities.SalesManagement;
using System.Transactions;

namespace smartshop.Data.Repositories
{
    internal class SaleRepository : ISaleRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IBankAccountRepsitory _bankAccountRepsitory;
        private readonly IHeadTransactionRepository _headTransactionRepository;

        public SaleRepository(ApplicationDbContext applicationDbContext, IBankAccountRepsitory bankAccountRepsitory, IHeadTransactionRepository headTransactionRepository)
        {
            _dbContext = applicationDbContext;
            this._bankAccountRepsitory = bankAccountRepsitory;
            this._headTransactionRepository = headTransactionRepository;
        }

        public bool AnyDueCollectionOrReturn(int businessId, int id)
        {
            return Query(businessId)
                .Include(p => p.DueCollections)
                .Include(p => p.SaleReturns)
                .Where(x => x.Id == id)
                .Any(p => p.DueCollections.Count > 0 || p.SaleReturns.Count > 0);
        }

        public int Create(Sale entity)
        {
            using (var ts = new TransactionScope())
            {
                _dbContext.Sales.Add(entity);
                _dbContext.SaveChanges();

                AccountStatusChanges(entity);

                ts.Complete();
                return entity.Id;
              
            }
        }

        public void Delete(int id)
        {
            var purchase = _dbContext.Sales.Find(id);
            //Balanced account calculation
            ReverseSaleCalculation(purchase);

            _dbContext.Sales.Remove(purchase);
            _dbContext.SaveChanges();
        }

        public bool Exists(int businessId, int id)
        {
            return Query(businessId).Any(p => p.Id == id);
        }

        public string GeneratSaleInvoiceNumber(int businessId)
        {
            var lastSale = _dbContext.Sales.OrderByDescending(x => x.Id).FirstOrDefault();
            int lastId = 0;

            if(lastSale != null)
                lastId = lastSale.Id;

            var invoicePrefix = _dbContext.BusinessConfigures.FirstOrDefault(x => x.Id == businessId).SalesInvoicePrefix;

            return $"{invoicePrefix}-{DateTime.UtcNow.ToString("yyy")}-{lastId + 1}";
        }

        public PaginationResponse<Sale> Get(int businessId, int currentPage, int pageSize, SalesQueryParams searchParams)
        {
            var purchases = Filter(businessId, searchParams);

            return new PaginationResponse<Sale>()
            {
                Rows = purchases.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList(),
                TotalRows = purchases.Count()
            };
        }

        public IEnumerable<Sale> Get(int businessId, int? storeId, int? customerId)
        {
            var query = Query(businessId)
                .Include(s => s.DueCollections)
                .AsQueryable();

            if(storeId != null)
                query = query.Where(s => s.StoreId == storeId);

            if (customerId != null)
                query = query.Where(c => c.CustomerId == customerId);

            return query.ToList();
        }

        public IEnumerable<Sale> Get(int businessId, SalesQueryParams searchParams)
        {
            return Filter(businessId, searchParams).ToList();
        }

        public Sale? Get(int businessId, int id)
        {
            return Query(businessId)
                .Include(x => x.SaleProducts)
                .ThenInclude(p => p.Stock)
                .ThenInclude(p => p.Product)
                .ThenInclude(p => p.Head)
                .Include(x => x.SaleProducts)
                .ThenInclude(x => x.UnitVariation)
                .ThenInclude(x => x.Unit)
                .Include(x => x.SaleProducts)
                .ThenInclude(p => p.Stock.Product.Brand)
                .Include(x => x.SaleProducts)
                .ThenInclude(p => p.Stock.Product.Category)
                .ThenInclude(c => c.Group)
                .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<SaleProduct> GetSaleProduct(int businessId, int saleId)
        {
            return _dbContext.SaleProducts
                .Include(sp => sp.Sale)
                .ThenInclude(sp => sp.Store)
                 .Include(p => p.Stock)
                .ThenInclude(p => p.Product)
                .ThenInclude(p => p.Head)
                .Include(x => x.UnitVariation)
                .ThenInclude(x => x.Unit)
                .Include(p => p.Stock.Product.Brand)
                .Include(p => p.Stock.Product.Category)
                .ThenInclude(c => c.Group)
                .Where(sp => sp.SaleId == saleId && sp.Sale.Store.BusinessId == businessId)
                .ToList();
        }

        public void Update(int id, Sale entity)
        {
            var sale = Query().FirstOrDefault(p => p.Id == id);

            //Balanced account calculation
            ReverseSaleCalculation(sale);

            _dbContext.SaleProducts.RemoveRange(sale.SaleProducts);

            sale.BankAccountId = entity.BankAccountId;
            sale.CustomerId = entity.CustomerId;
            sale.StoreId = entity.StoreId;
            sale.Date = entity.Date;
            sale.Discount = entity.Discount;
            sale.DiscountType = entity.DiscountType;
            sale.Overhead = entity.Overhead;
            sale.Paid = entity.Paid;
            sale.Vat = entity.Vat;

            _dbContext.SaleProducts.AddRange(entity.SaleProducts);

            _dbContext.Entry(sale).State = EntityState.Modified;
            _dbContext.SaveChanges();

            AccountStatusChanges(sale);
        }

        private IQueryable<Sale> Filter(int businessId, SalesQueryParams queryParams)
        {
            var query = Query(businessId);

            if (queryParams.CustomerId != null)
                query = query.Where(p => p.CustomerId == queryParams.CustomerId);

            if (queryParams.StoreId != null)
                query = query.Where(p => p.StoreId == queryParams.StoreId);

            if (queryParams.InvoiceNumber != null)
                query = query.Where(p => p.InvoiceNumber.Contains(queryParams.InvoiceNumber));

            if (queryParams.Date != null)
                query = query.Where(p => p.Date.Date == queryParams.Date.Value.Date);
            
            if (queryParams.BankAccountId != null)
                query = query.Where(p => p.BankAccountId == queryParams.BankAccountId);

            return query;
        }

        private IQueryable<Sale> Query(int? businessId = null)
        {
            var query = _dbContext.Sales
                 .Include(x => x.Customer)
                 .ThenInclude(s => s.Head)
                 .Include(x => x.Store)
                 .Include(x => x.BankAccount)
                 .ThenInclude(x => x.Head)
                 .Include(x => x.SaleProducts)
                 .AsQueryable();

            if (businessId != null)
                query = query.Where(x => x.Store.BusinessId == businessId);

            return query.OrderByDescending(x => x.Id);
        }


        /// <summary>
        ///  Balanced account when sale delete or Edit
        /// </summary>
        /// <param name="sale"></param>
        private void ReverseSaleCalculation(Sale sale)
        {
            //Update Bank Account
            var bankAccount = _bankAccountRepsitory.GetById(sale.BankAccountId);
            bankAccount.Balance -= sale.Paid;
            _bankAccountRepsitory.Update(bankAccount);

            _headTransactionRepository.DeleteBySale(sale.Id);
        }

        /// <summary>
        /// Account status changed when sale create or update
        /// BankAccount Balance Calculate
        /// Head Transactions added
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="businessId"></param>
        private void AccountStatusChanges(Sale entity)
        {
            //Update Bank Account
            var bankAccount = _bankAccountRepsitory.GetById(entity.BankAccountId);
            bankAccount.Balance += entity.Paid;
            _bankAccountRepsitory.Update(bankAccount);

            //Add Heads
            var listOfHeadTransactions = new List<HeadTransaction>()
                {
                    new HeadTransaction(){
                        SaleId = entity.Id,
                        Amount = entity.GetPayableAmount(),
                        Date = entity.Date,
                        Descriptions = $"Sale bill for sale invoice-{entity.InvoiceNumber}",
                        Type = TransactionType.Credit,
                        HeadId = entity.CustomerId,
                        AccountTransactions = new List<AccountTransaction>()
                        {
                            new AccountTransaction(){AccountId = TransactionAccount.PRODUCT, Operator = Operators.MINUS},
                            new AccountTransaction(){AccountId = TransactionAccount.ACCOUNT_RECEIVABLE, Operator = Operators.PLUS},
                        }
                    },

                    //Paid Transaction Add
                    new HeadTransaction(){
                        SaleId = entity.Id,
                        Amount = entity.Paid,
                        Date = entity.Date,
                        Descriptions = $"Sale paid for sale invoice-{entity.InvoiceNumber}",
                        Type = TransactionType.Debit,
                        HeadId = entity.CustomerId,
                        AccountTransactions = new List<AccountTransaction>()
                        {
                            new AccountTransaction(){AccountId = TransactionAccount.CASH, Operator = Operators.PLUS},
                            new AccountTransaction(){AccountId = TransactionAccount.ACCOUNT_RECEIVABLE, Operator = Operators.MINUS},
                        }
                    },
                      new HeadTransaction()
                      {
                          SaleId = entity.Id,
                          Amount= entity.Paid,
                          Date = entity.Date,
                          Descriptions = $"Sale paid for sale invoice-{entity.InvoiceNumber}",
                          Type = TransactionType.Credit,
                          HeadId = entity.BankAccountId,
                      }
                };

            _headTransactionRepository.Create(listOfHeadTransactions);
        }
    }
}
