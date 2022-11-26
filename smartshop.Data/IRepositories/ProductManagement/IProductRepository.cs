using smartshop.Common.QueryParams;

namespace smartshop.Data.IRepositories
{
    public interface IProductRepository
    {
        PaginationResponse<Product> Get(int businessId, int currentPage, int pageSize, ProductQueryParams queryParams);
        IEnumerable<Product> Get(int businessId, ProductQueryParams queryParams);
        Product? Get(int businessId, int id);
        bool Exists(int businessId, int id);
        bool Exists(int businessId, string name, int? id = null);
        int Create(Product entity);
        void Update(int id, Product entity);
        void Delete(int id);
    }
}
