

namespace smartshop.Data.IRepositories
{
    public interface ICategoryRepository
    {
        PaginationResponse<Category> Get(int businessId, int currentPage, int pageSize);
        IEnumerable<Category> GetDdl(int businessId, int groupId);
        Category? Get(int businessId, int id);
        bool Exists(int businessId, int id);
        bool Exists(int businessId, string name, int? id = null);
        int Create(Category entity);
        void Update(int id, Category entity);
        void Delete(int id);
        IEnumerable<Category> Get(int businessId);
    }
}
