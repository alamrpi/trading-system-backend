using smartshop.Business.Dtos.Users;

namespace smartshop.Business.IServices.Auth
{
    public interface IAuthenticationService
    {
        string? GetToken(string userName, string password);

        bool UserExistsByEmail(string email);

        bool UserExistsByPhone(string phoneNumber);
        void ChangePassword(string userId, string oldPassword, string newPassword);
        ProfileInfoDto GetProfileInfo(string userId);
        void UpdateProfile(string userId, UpdateProfileViewModel model);
    }
}
