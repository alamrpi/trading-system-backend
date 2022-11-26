
namespace smartshop.Data.IRepositories
{
    public interface IDesignationRepository
    {
        IEnumerable<Designation> Get(int businessId);
        PaginationResponse<Designation> Get(int businessId, int currentPage, int pageSize);
        Designation? Get(int businessId, int id);
        bool Exits(int businessId, string name, int? id = null);
        int? CreateOrUpdate(Designation designation);
        void Delete(int businessId, int id);
        bool Exits(int businessId, int id);
    }
}
