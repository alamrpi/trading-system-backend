using smartshop.Common.QueryParams;
using smartshop.Entities.PurchaseManagement;
using smartshop.Entities.SalesManagement;
using smartshop.Entities.Stocks;

namespace smartshop.Data.Repositories
{
    internal class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public StockRepository(ApplicationDbContext applicationDbContext)
        {
            _dbContext = applicationDbContext;
        }

        public int CreateDamage(StockDamage damage)
        {
            _dbContext.StockDamages.Add(damage);
            _dbContext.SaveChanges();
            return damage.Id;
        }

        public bool ExistsStock(int businessId, int stockId)
        {
            return _dbContext.Stocks
                .Include(s => s.Store)
                .Any(s => s.Store.BusinessId == businessId && s.Id == stockId);
        }

        public IEnumerable<StockDamage> GetDamageReports(int businessId, StockOrDamageQueryParams queryParams)
        {
            return FilterStockDamages(businessId, queryParams).ToList();
        }

        public PaginationResponse<StockDamage> GetDamages(int businessId, int currentPage, int pageSize, StockOrDamageQueryParams queryParams)
        {
            var query =  FilterStockDamages(businessId, queryParams).ToList();

            return new PaginationResponse<StockDamage>()
            {
                TotalRows = query.Count,
                Rows = query.Skip((currentPage - 1) * pageSize).Take(pageSize),
            };
        }

       
        public IEnumerable<Stock> GetReports(int businessId, StockOrDamageQueryParams queryParams)
        {
           return FilterStocks(businessId, queryParams);
        }

        public PaginationResponse<Stock> Gets(int businessId, int currentPage, int pageSize, StockOrDamageQueryParams queryParams)
        {
            var query = FilterStocks(businessId, queryParams).ToList();

            return new PaginationResponse<Stock>()
            {
                TotalRows = query.Count,
                Rows = query.Skip((currentPage - 1) * pageSize).Take(pageSize),
            };
        }

        public void ReverseStockForPurchase(int id)
        {
            var purchaseProducts = _dbContext.PurchaseProducts.Include(x => x.Purchase).Where(x => x.PurchaseId == id).ToList();
            foreach (var item in purchaseProducts)
            {
                var stock = _dbContext.Stocks.FirstOrDefault(x => x.ProductId == item.ProductId && x.StoreId == item.Purchase.StoreId);
                stock.StockQty -= CalculateBaseQnty(item.UnitVariationId, (item.Qty + item.BonusQty));
                _dbContext.Entry(stock).State = EntityState.Modified;
            }

            _dbContext.SaveChanges();
        }

        public void ReverseStockForPurchaseReturn(int id)
        {
            var purchaseReturnProdcts = _dbContext.PurchaseReturnProducts.Where(x => x.PurchaseReturnId == id).ToList();
            PurchaseReturnStockUpdate(purchaseReturnProdcts, true);
        }

        public void UpdateStockForPurchaseReturn(IEnumerable<PurchaseReturnProduct> products)
        {
            PurchaseReturnStockUpdate(products);
        }

        public void UpdateStockWhenPurchase(IEnumerable<PurchaseProduct> purchaseProducts, int storeId)
        {
            foreach (var product in purchaseProducts)
            {
                var stock = _dbContext.Stocks.FirstOrDefault(x => x.ProductId == product.ProductId && x.StoreId == storeId);
                var baseQty = CalculateBaseQnty(product.UnitVariationId, (product.Qty + product.BonusQty));
                if (stock != null)
                {
                    stock.StockQty += baseQty;
                    stock.TradePrice = product.TradePrice;
                    _dbContext.Entry(stock).State = EntityState.Modified;
                }
                else
                {
                    _dbContext.Stocks.Add(new Stock(storeId, product.ProductId, baseQty, product.TradePrice));
                }
            }
            _dbContext.SaveChanges();
        }

        private IQueryable<Stock> FilterStocks(int businessId, StockOrDamageQueryParams queryParams)
        {
            var query = _dbContext.Stocks
                .Include(x => x.Store)
                .Include(x => x.Product.Head)
                .Include(x => x.Product)
                .ThenInclude(x => x.Brand)
                .Include(x => x.Product.Unit)
                .Include(x => x.Product.Category)
                .ThenInclude(x => x.Group)
                .Where(x => x.Store.BusinessId == businessId)
                .AsQueryable();

            if (queryParams.StoreId != null) query = query.Where(sd => sd.StoreId == queryParams.StoreId);
            if (queryParams.ProductId != null) query = query.Where(sd => sd.ProductId == queryParams.ProductId);
            if (queryParams.BrandId != null) query = query.Where(sd => sd.Product.BrandId == queryParams.BrandId);
            if (queryParams.UnitId != null) query = query.Where(sd => sd.Product.UnitId == queryParams.UnitId);
            if (queryParams.CategoryId != null) query = query.Where(sd => sd.Product.CategoryId == queryParams.CategoryId);
            if (queryParams.GroupId != null) query = query.Where(sd => sd.Product.Category.GroupId == queryParams.GroupId);

            return query;
        }
        
        private IQueryable<StockDamage> FilterStockDamages(int businessId, StockOrDamageQueryParams queryParams)
        {
            var query = _dbContext.StockDamages
                .Include(x => x.Stock)
                .ThenInclude(x => x.Store)
                .Include(x => x.Stock.Product)
                .ThenInclude(x => x.Head)
                .Include(x => x.Stock.Product.Brand)
                .Include(x => x.Stock.Product.Unit)
                .Include(x => x.Stock.Product.Category)
                .ThenInclude(x => x.Group)
                .Where(x => x.Stock.Store.BusinessId == businessId)
                .AsQueryable();

            if (queryParams.StoreId != null) query = query.Where(sd => sd.Stock.StoreId == queryParams.StoreId);
            if (queryParams.ProductId != null) query = query.Where(sd => sd.Stock.ProductId == queryParams.ProductId);
            if (queryParams.BrandId != null) query = query.Where(sd => sd.Stock.Product.BrandId == queryParams.BrandId);
            if (queryParams.UnitId != null) query = query.Where(sd => sd.Stock.Product.UnitId == queryParams.UnitId);
            if (queryParams.CategoryId != null) query = query.Where(sd => sd.Stock.Product.CategoryId == queryParams.CategoryId);
            if (queryParams.GroupId != null) query = query.Where(sd => sd.Stock.Product.Category.GroupId == queryParams.GroupId);

            return query;
        }
        private decimal CalculateBaseQnty(int unitVariationId, decimal qty)
        {
            var unitVariations = _dbContext.UnitVariations.FirstOrDefault(x => x.Id == unitVariationId);
            if (unitVariations != null)
                return (decimal)unitVariations.Qnty * qty;

            throw new Exception("Unit variation not found when update stock!");
        }

        private void PurchaseReturnStockUpdate(IEnumerable<PurchaseReturnProduct> products, bool isReverse = false)
        {
            foreach (var item in products)
            {
                var purchaseProduct = _dbContext.PurchaseProducts
                    .Include(x => x.Purchase)
                    .FirstOrDefault(p => p.Id == item.PurchaseProductId);

                var stock = _dbContext.Stocks.FirstOrDefault(x => x.ProductId == purchaseProduct.ProductId && x.StoreId == purchaseProduct.Purchase.StoreId);
                if (stock == null)
                    throw new Exception("Stock not found when purchase return.");

                if (isReverse)
                    stock.StockQty += CalculateBaseQnty(purchaseProduct.UnitVariationId, item.Qty);
                else
                    stock.StockQty -= CalculateBaseQnty(purchaseProduct.UnitVariationId, item.Qty);

                _dbContext.Entry(stock).State = EntityState.Modified;

            }
            _dbContext.SaveChanges();
        }

        public void UpdateStockWhenSale(IEnumerable<SaleProduct> saleProducts)
        {
            UpdateStockForSale(saleProducts, false);
        }

        public void ReverseStockForSale(int id)
        {
            var saleProducts = _dbContext.SaleProducts.Where(x => x.SaleId == id).ToList();
            UpdateStockForSale(saleProducts, true);
        }

        private void UpdateStockForSale(IEnumerable<SaleProduct> saleProducts, bool isReverse)
        {
            foreach (var item in saleProducts)
            {
                var stock = _dbContext.Stocks.FirstOrDefault(x => x.Id == item.StockId);
                if (stock == null)
                    throw new Exception("Stock not found when purchase return.");

                if (isReverse)
                    stock.StockQty += CalculateBaseQnty(item.UnitVariationId, item.Qnty);
                else
                    stock.StockQty -= CalculateBaseQnty(item.UnitVariationId, item.Qnty);

                _dbContext.Entry(stock).State = EntityState.Modified;
            }

            _dbContext.SaveChanges();
        }

        public IEnumerable<Stock> GetForDdl(int businessId, int storeId)
        {
            return _dbContext.Stocks
                .Include(s => s.Product)
                .ThenInclude(p => p.Head)
                .Include(s => s.Product.Category)
                .ThenInclude(s => s.Group)
                .Include(s => s.Product.Brand)
                .Include(s => s.Product.Unit)
                .Include(s => s.Store)
                .Where(x => x.StoreId == storeId && x.Store.BusinessId == businessId && x.StockQty > 0)
                .ToList();
        }

        public void ReverseStockForSaleReturn(int id)
        {
            var purchaseReturnProdcts = _dbContext.SaleReturnProducts.Where(x => x.SaleReturnId == id).ToList();
            SaleReturnStockUpdate(purchaseReturnProdcts, true);
        }

        public void UpdateStockForSaleReturn(IEnumerable<SaleReturnProduct> products)
        {
            SaleReturnStockUpdate(products);
        }

        private void SaleReturnStockUpdate(IEnumerable<SaleReturnProduct> products, bool isReverse = false)
        {
            foreach (var item in products)
            {
                var saleProduct = _dbContext.SaleProducts
                    .Include(x => x.Sale)
                    .FirstOrDefault(p => p.Id == item.SaleProductId);

                var stock = _dbContext.Stocks.FirstOrDefault(x => x.Id == saleProduct.StockId && x.StoreId == saleProduct.Sale.StoreId);
                if (stock == null)
                    throw new Exception("Stock not found when purchase return.");

                if (isReverse)
                    stock.StockQty -= CalculateBaseQnty(saleProduct.UnitVariationId, item.Qty);
                else
                    stock.StockQty += CalculateBaseQnty(saleProduct.UnitVariationId, item.Qty);

                _dbContext.Entry(stock).State = EntityState.Modified;

            }
            _dbContext.SaveChanges();
        }

        public int GetUnitId(int businessId, int stockId)
        {
            return _dbContext.Stocks
                .Include(x => x.Product)
                .FirstOrDefault(x => x.Id == stockId)
                .Product.UnitId;
        }
    }
}
