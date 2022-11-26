using smartshop.Business.Dtos.StockInventory;
using smartshop.Common.QueryParams;

namespace smartshop.Business.IServices
{
    public interface IStockService
    {
        PaginationResponse<StockDto> Gets(int businessId, int currentPage, int pageSize, StockOrDamageQueryParams queryParams);
        IEnumerable<StockDto> GetReports(int businessId, StockOrDamageQueryParams queryParams);
        bool ExistsStock(int businessId, int stockId);

        PaginationResponse<StockDamageDto> GetDamages(int businessId, int currentPage, int pageSize, StockOrDamageQueryParams queryParams);
        IEnumerable<StockDamageDto> GetDamageReports(int businessId, StockOrDamageQueryParams queryParams);
        int CreateDamage(StockDamageViewModel model, string userId, string clientIp);
        IEnumerable<DropdownDto> GetForDdl(int v, int storeId);
        StockFilterDdlDto GetStockDdlData(int businessId);
    }
}
