using smartshop.Business.ViewModels.ProductManagement;
using smartshop.Common.QueryParams;

namespace smartshop.Business.IServices
{
    public interface IProductService
    {
        PaginationResponse<ProductDto> Get(int businessId, int currentPage, int pageSize, ProductQueryParams queryParams);
        ProductDto? Get(int businessId, int id);
        IEnumerable<DropdownDto> Get(int businessId, ProductQueryParams queryParams);
        bool Exists(int businessId, int id);
        bool Exists(int businessId, string name, int? id = null);
        int Create(int businessId, ProductViewModel entity);
        void Update(int id, ProductViewModel entity);
        void Delete(int id);
        DropdownDataForProductViewModel GetDdlData(int v);
    }
}
