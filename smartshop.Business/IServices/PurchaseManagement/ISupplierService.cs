namespace smartshop.Business.IServices
{
    public interface ISupplierService
    {
        PaginationResponse<SupplierDto> Get(int businessId, int currentPage, int pageSize);
        SupplierDto? Get(int businessId, int id);
        IEnumerable<DropdownDto> Get(int businessId);
        bool Exists(int businessId, int id);
        bool Exists(int businessId, string name, int? id = null);
        int Create(int businessId, SupplierViewModel entity);
        void Update(int id, SupplierViewModel entity);
        void Delete(int id);
    }
}
