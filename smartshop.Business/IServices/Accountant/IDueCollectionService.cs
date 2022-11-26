using smartshop.Common.QueryParams;

namespace smartshop.Business.IServices
{
    public interface IDueCollectionService
    {
        PaginationResponse<DueCollectionDto> Get(int businessId, DueCollectionQueryParams queryParams);
        DueCollectionDto? Get(int businessId, int id);
        bool Exists(int businessId, int id);
        int Create(DueCollectionViewModel entity, string ip, string userId, int businessId);
        void Update(int id, DueCollectionViewModel entity);
        void Delete(int id);
        DueCollectionDdlDataDto GetDdlData(int businessId);
    }
}
