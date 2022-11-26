using smartshop.Business.Dtos.Users;

namespace smartshop.Business.IServices.Users
{
    public interface IAdminService
    {
        IEnumerable<ApplicationUserDto> Gets(int page, int pageSize);
        ApplicationUserDto? Get(string id);
        int TotalCount();
        string Create(ApplicationUserViewModel model);
        bool Update(string id, ApplicationUserViewModel model);
        void Active(string id);
        void Deactive(string id);
        bool AnyAdminByBusiness(int storeId);
    }
}
