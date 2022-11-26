namespace smartshop.Business.IServices
{
    public interface IBankAccountService
    {
        PaginationResponse<BankAccountDto> Get(int businessId, int currentPage, int pageSize);
        BankAccountDto? Get(int businessId, int id);
        IEnumerable<DropdownDto> Get(int businessId);
        bool Exists(int businessId, int id);
        bool Exists(int businessId, string name, int? id = null);
        int Create(int businessId, BankAccountViewModel entity);
        void Update(int id, BankAccountViewModel entity);
        void Delete(int id);
    }
}
