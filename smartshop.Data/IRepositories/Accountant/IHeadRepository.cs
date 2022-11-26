using smartshop.Entities.Accounting;

namespace smartshop.Data.IRepositories.Accountant
{
    public interface IHeadRepository
    {
        IEnumerable<Head> Get(int businessId);
        PaginationResponse<Head> Get(int businessId, int currentPage, int pageSize);
        Head? Get(int businessId, int id);
        bool Exits(int businessId, string name, int? id = null);
        int? CreateOrUpdate(Head designation);
        void Delete(int businessId, int id);
        bool Exits(int businessId, int id);
    }
}
