using smartshop.Business.Dtos.Business;
using smartshop.Business.ViewModels.Business;

namespace smartshop.Business.IServices.Businesses
{
    public interface IStoreService
    {
        IEnumerable<StoreDto> Gets(int page, int pageSize, int? businessId);
        IEnumerable<DropdownDto> Gets(int? businessId);
        StoreDetailDto? Get(int id);

        int TotalCount();

        int Create(StoreViewModel entity);

        bool Update(int id, StoreViewModel entity);

        bool Delete(int id);

        void Active(int id);
        void Deactive(int id, DeactiveViewModel model);

    }
}
