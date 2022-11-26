using smartshop.Entities.Common;
using smartshop.Entities.HumanResource;

namespace smartshop.Data.IRepositories.Auth
{
    public interface IAuthenticationRepository
    {
        ApplicationUser? GetUserByUserName(string userName);

        string? GetToken(string userName, string password);

        bool UserExistsByEmail(string email);

        bool UserExistsByPhone(string phoneNumber);
        void ChangePassword(string userId, string oldPassword, string newPassword);
        Employee? GetProfileInfo(string userId);
        void UpdateProfile(string userId, ApplicationUser applicationUser);
    }
}
