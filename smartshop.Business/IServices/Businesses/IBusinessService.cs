using smartshop.Business.Dtos.Business;
using smartshop.Business.ViewModels.Business;

namespace smartshop.Business.IServices.Businesses
{
    public interface IBusinessService
    {
        IEnumerable<BusinessDto> Gets(int page, int pageSize);
        IEnumerable<DropdownDto> Gets();

        BusinessDetailsDto? Get(int id);

        int TotalCount();

        int Create(BusinessViewModel entity);

        bool Update(int id, BusinessViewModel entity);

        bool Delete(int id);

        void Active(int id);
        void Deactive(int id, DeactiveViewModel model);
        bool Update(int v, BusinessConfigureViewModel model);

        BusinessConfigureDto? GetBusinessConfigure(int id);
    }
}
