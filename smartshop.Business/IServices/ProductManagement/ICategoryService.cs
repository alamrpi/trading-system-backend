
namespace smartshop.Business.IServices
{
    public interface ICategoryService
    {
        PaginationResponse<CategoryDto> Get(int businessId, int currentPage, int pageSize);
        CategoryDto? Get(int businessId, int id);
        IEnumerable<DropdownDto> GetDropdown(int businessId, int groupId);
        bool Exists(int businessId, int id);
        bool Exists(int businessId, string name, int? id = null);
        int Create(int businessId, CategoryViewModel entity);
        void Update(int id, CategoryViewModel entity);
        void Delete(int id);
    }
}
