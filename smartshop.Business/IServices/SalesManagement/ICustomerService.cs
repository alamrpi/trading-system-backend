namespace smartshop.Business.IServices
{
    public interface ICustomerService
    {
        PaginationResponse<CustomerDto> Get(int businessId, int currentPage, int pageSize);
        CustomerDto? Get(int businessId, int id);
        IEnumerable<DropdownDto> Get(int businessId);
        bool Exists(int businessId, int id);
        bool Exists(int businessId, string name, int? id = null);
        int Create(int businessId, CustomerViewModel entity);
        void Update(int id, CustomerViewModel entity);
        void Delete(int id);
    }
}
