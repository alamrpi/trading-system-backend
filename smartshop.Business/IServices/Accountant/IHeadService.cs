namespace smartshop.Business.IServices
{
    public interface IHeadService 
    {
        IEnumerable<DropdownDto> Get(int businessId);
        PaginationResponse<HeadDto> Get(int businessId, int currentPage, int pageSize);
        HeadDto? Get(int businessId, int id);
        bool Exits(int businessId, string name, int? id = null);
        int? CreateOrUpdate(int businessId, HeadViewModel head, int id = 0);
        void Delete(int businessId, int id);
        bool Exits(int businessId, int id);
    }
}
