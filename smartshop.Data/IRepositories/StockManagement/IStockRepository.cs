using smartshop.Entities.PurchaseManagement;
using smartshop.Entities.SalesManagement;
using smartshop.Entities.Stocks;

namespace smartshop.Data.IRepositories
{
    public interface IStockRepository
    {
        PaginationResponse<Stock> Gets(int businessId, int currentPage, int pageSize, StockOrDamageQueryParams queryParams);
        IEnumerable<Stock> GetReports(int businessId, StockOrDamageQueryParams queryParams);
        bool ExistsStock(int businessId, int stockId);

        PaginationResponse<StockDamage> GetDamages(int businessId, int currentPage, int pageSize, StockOrDamageQueryParams queryParams);
        IEnumerable<StockDamage> GetDamageReports(int businessId, StockOrDamageQueryParams queryParams);
        int CreateDamage(StockDamage damage);
        
        void UpdateStockWhenPurchase(IEnumerable<PurchaseProduct> purchaseProducts, int storeId);
        void ReverseStockForPurchase(int id);

        void UpdateStockForPurchaseReturn(IEnumerable<PurchaseReturnProduct> products);
        void ReverseStockForPurchaseReturn(int id);
        void UpdateStockWhenSale(IEnumerable<SaleProduct> saleProducts);
        void UpdateStockForSaleReturn(IEnumerable<SaleReturnProduct> saleReturnProducts);
        void ReverseStockForSale(int id);
        void ReverseStockForSaleReturn(int id);
        IEnumerable<Stock> GetForDdl(int businessId, int storeId);
        int GetUnitId(int businessId, int productId);
    }
}
