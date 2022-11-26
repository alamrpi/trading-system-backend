
namespace smartshop.Data.IRepositories
{
    public interface IBrandRepository
    {
        PaginationResponse<Brand> Get(int businessId, int currentPage, int pageSize);
        IEnumerable<Brand> Get(int businessId);
        Brand? Get(int businessId, int id);
        bool Exists(int businessId, int id);
        bool Exists(int businessId, string name, int? id = null);
        int Create(Brand entity);
        void Update(int id, Brand entity);
        void Delete(int id);
     
    }
}
