using smartshop.Business.ViewModels.Users;

namespace smartshop.Business.IServices.Users
{
    public interface IUserService
    {
        PermissionDto? Get(string userId);
        void Permit(PermissionViewModel model);
        bool Exits(string userId);
    }
}
