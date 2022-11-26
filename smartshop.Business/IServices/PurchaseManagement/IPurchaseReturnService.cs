using smartshop.Common.QueryParams.Purchase;

namespace smartshop.Business.IServices
{
    public interface IPurchaseReturnService
    {
        PaginationResponse<PurchaseReturnDto> Get(int businessId, int currentPage, int pageSize, PurchaseQueryParams searchParams);
        IEnumerable<PurchaseReturnDto> Get(int businessId, PurchaseQueryParams searchParams);
        PurchaseReturnDetailsDto? Get(int businessId, int id);
        bool Exists(int businessId, int id);
        int Create(PurchaseReturnViewModel entity, string userId, string clientIp);
        void Update(int id, PurchaseReturnViewModel entity);
        void Delete(int id);
    }
}
