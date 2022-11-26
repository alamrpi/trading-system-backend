using smartshop.Entities.Common;
using smartshop.Entities.HumanResource;

namespace smartshop.Data.IRepositories.Users
{
    public interface IAdminRepository 
    {
        IEnumerable<ApplicationUser> Gets(int page, int pageSize);
        ApplicationUser? Get(string id);

        int TotalCount();

        ApplicationUser Create(ApplicationUser entity);

        void Update(ApplicationUser entity);
        Employee? GetEmployee(string id);
        bool AnyAdminByBusiness(int storeId);
    }
}
