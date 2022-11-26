using smartshop.Common.Dto;
using smartshop.Entities.Common;

namespace smartshop.Data.IRepositories
{
    public interface IUserRepository
    {
        string? Create(ApplicationUser applicationUser);
        bool UserExistsByEmail(string email);
        bool UserExistsByPhone(string phoneNumber);
        void Permit(string userId, List<string> roles);
        PermissionDto? GetUserRoles(string userId);
        bool Exists(string userId);
    }
}
