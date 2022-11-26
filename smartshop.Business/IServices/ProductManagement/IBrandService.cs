namespace smartshop.Business.IServices
{
    public interface IBrandService
    {
        PaginationResponse<BrandDto> Get(int businessId, int currentPage, int pageSize);
        BrandDto? Get(int businessId, int id);
        IEnumerable<DropdownDto> Get(int businessId);
        bool Exists(int businessId, int id);
        bool Exists(int businessId, string name, int? id = null);
        int Create(int businessId, BrandViewModel entity);
        void Update(int id, BrandViewModel entity);
        void Delete(int id);
    }
}
