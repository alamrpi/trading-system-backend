using smartshop.Business.ViewModels.ProductManagement;

namespace smartshop.Business.IServices
{
    public interface IUnitService
    {
        PaginationResponse<UnitDto> Get(int businessId, int currentPage, int pageSize);
        UnitDto? Get(int businessId, int id);
        IEnumerable<DropdownDto> Get(int businessId);
        IEnumerable<DropdownDto> GetVariations(int businessId, int unitId);
        bool Exists(int businessId, int id);
        bool Exists(int businessId, string name, int? id = null);
        int Create(int businessId, UnitWithVariationViewModel entity);
        void CreateUnitVariation(int unitId, List<UnitVariationViewModel> variations);
        void Update(int id, UnitViewModel entity);
        void Delete(int id);
        void DeleteUnitVariation(int variationId);
        IEnumerable<DropdownDto> GetVariationsByProductId(int v, int productId);
        IEnumerable<DropdownDto> GetVariationsByStockId(int v, int productId);
    }
}
