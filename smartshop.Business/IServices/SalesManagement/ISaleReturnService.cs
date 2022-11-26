using smartshop.Common.QueryParams;

namespace smartshop.Business.IServices
{
    public interface ISaleReturnService
    {
        PaginationResponse<SaleReturnDto> Get(int businessId, int currentPage, int pageSize, SalesQueryParams searchParams);
        IEnumerable<SaleReturnDto> Get(int businessId, SalesQueryParams searchParams);
        SaleReturnDetailsDto? Get(int businessId, int id);
        bool Exists(int businessId, int id);
        int Create(SalesReturnViewModel entity, string userId, string clientIp, int businessId);
        void Update(int id, SalesReturnViewModel entity);
        void Delete(int id);
    }
}
