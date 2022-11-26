using smartshop.Entities.Accounting;

namespace smartshop.Data.IRepositories
{
    public interface IDueCollectionRepository
    {
        PaginationResponse<DueCollection> Get(int businessId, DueCollectionQueryParams queryParams);
        DueCollection? Get(int businessId, int id);
        bool Exists(int businessId, int id);
        int Create(DueCollection entity);
        void Update(int id, DueCollection entity);
        void Delete(int id);
        string GenerateSerialNo(int businessId);
    }
}
