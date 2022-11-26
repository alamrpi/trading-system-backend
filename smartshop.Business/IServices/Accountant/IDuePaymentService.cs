using smartshop.Common.QueryParams;

namespace smartshop.Business.IServices
{
    public interface IDuePaymentService
    {
        PaginationResponse<DuePaymentDto> Get(int businessId, DuePaymentQueryParams queryParams);
        DuePaymentDto? Get(int businessId, int id);
        bool Exists(int businessId, int id);
        int Create(int businessId, DuePaymentViewModel entity, string ip, string userId);
        void Update(int id, DuePaymentViewModel entity);
        void Delete(int id);
        DuePaymentDdlDataDto GetDdlData(int businessId);
    }
}
