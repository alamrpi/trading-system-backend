namespace smartshop.Data.IRepositories
{
    public interface ISupplierRepository
    {
        PaginationResponse<Supplier> Get(int businessId, int currentPage, int pageSize);
        IEnumerable<Supplier> Get(int businessId);
        Supplier? Get(int businessId, int id);
        bool Exists(int businessId, int id);
        bool Exists(int businessId, string name, int? id = null);
        int Create(Supplier entity);
        void Update(int id, Supplier entity);
        void Delete(int id);
        int TotalCount(int businessId);
    }
}
