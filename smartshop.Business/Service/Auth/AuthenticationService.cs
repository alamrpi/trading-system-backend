using smartshop.Business.Dtos.Users;
using smartshop.Business.IServices.Auth;
using smartshop.Data.IRepositories.Auth;
using smartshop.Entities.Common;

namespace smartshop.Business.Service.Auth
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository _authenticationRepository;

        public AuthenticationService(IAuthenticationRepository authenticationRepository)
        {
            this._authenticationRepository = authenticationRepository;
        }

        public void ChangePassword(string userId, string oldPassword, string newPassword)
        {
            _authenticationRepository.ChangePassword(userId, oldPassword, newPassword);
        }

        public ProfileInfoDto GetProfileInfo(string userId)
        {
            var employee = _authenticationRepository.GetProfileInfo(userId);
            if (employee == null)
                throw new Exception("Employee information not found");

            return new ProfileInfoDto()
            {
                BusinessName = employee.Designation.Business.Name,
                BusinessSlogan = employee.Designation.Business.Objective,
                BusinessEmail = employee.Designation.Business.Email,
                BusinessLogo = employee.Designation.Business.Logo,
                Designation = employee.Designation.Name,
                FirstName = employee.User.FirstName,
                LastName = employee.User.LastName,
                Email = employee.User.Email,
                Phone = employee.User.PhoneNumber,
                ProfilePic = employee.User.PhotoUrl,
                BusinessAddress = employee.Designation.Business.Address,
                BusinessContact = employee.Designation.Business.ContactNo,
            };
        }

        public string? GetToken(string userName, string password) 
            => _authenticationRepository.GetToken(userName, password);

        public void UpdateProfile(string userId, UpdateProfileViewModel model)
        {
            _authenticationRepository.UpdateProfile(userId, new ApplicationUser(model.FirstName, model.LastName, "", model.Email, model.Phone, false));
        }

        public bool UserExistsByEmail(string email) 
            => _authenticationRepository.UserExistsByEmail(email);

        public bool UserExistsByPhone(string phoneNumber) 
            => _authenticationRepository.UserExistsByPhone(phoneNumber);
    }
}
