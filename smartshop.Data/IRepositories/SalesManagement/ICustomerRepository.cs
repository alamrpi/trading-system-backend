using smartshop.Entities.SalesManagement;

namespace smartshop.Data.IRepositories
{
    public interface ICustomerRepository
    {
        PaginationResponse<Customer> Get(int businessId, int currentPage, int pageSize);
        IEnumerable<Customer> Get(int businessId);
        Customer? Get(int businessId, int id);
        bool Exists(int businessId, int id);
        bool Exists(int businessId, string name, int? id = null);
        int Create(Customer entity);
        void Update(int id, Customer entity);
        void Delete(int id);
        int TotalCount(int businessId);
    }
}
