namespace smartshop.Business.IServices
{
    public interface IDesignationService
    {
        IEnumerable<DropdownDto> Get(int businessId);
        PaginationResponse<DesignationDto> Get(int businessId, int currentPage, int pageSize);
        DesignationDto? Get(int businessId, int id);
        bool Exits(int businessId, string name, int? id = null);
        int? CreateOrUpdate(int businessId, DesignationViewModel designation, int id = 0);
        void Delete(int businessId, int id);
        bool Exits(int businessId, int id);
    }
}
