namespace smartshop.Business.IServices
{
    public interface IInvestorService
    {
        PaginationResponse<InvestorDto> Get(int businessId, int currentPage, int pageSize);
        InvestorDto? Get(int businessId, int id);
        IEnumerable<DropdownDto> Get(int businessId);
        bool Exists(int businessId, int id);
        int Create(int businessId, InvestorViewModel entity);
        void Update(int id, InvestorViewModel entity);
        void Delete(int id);
    }
}
