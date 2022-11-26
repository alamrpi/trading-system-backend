using smartshop.Business.Dtos.SalesManagement;
using smartshop.Common.QueryParams;

namespace smartshop.Business.IServices
{
    public interface ISaleService
    {
        PaginationResponse<SaleDto> Get(int businessId, int currentPage, int pageSize, SalesQueryParams searchParams);
        IEnumerable<DropdownDto> Get(int businessId, int? storeId, int? customerId);
        IEnumerable<SaleDto> Get(int businessId, SalesQueryParams searchParams);
        SaleDetailsDto? Get(int businessId, int id);

        bool Exists(int businessId, int id);
        bool AnyDueCollectionOrReturn(int businessId, int id);
        int Create(SaleViewModel entity, string userId, string clientIp, int businessId);
        void Update(int id, SaleViewModel entity);
        void Delete(int id);
        SalesDdlDto GetDdlData(int businessId);
        IEnumerable<SaleProductDto> GetSaleProducts(int businessId, int id);
    }
}
