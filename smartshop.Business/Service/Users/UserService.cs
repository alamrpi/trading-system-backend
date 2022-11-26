using smartshop.Business.IServices.Users;
using smartshop.Business.ViewModels.Users;

namespace smartshop.Business.Service.Users
{
    internal class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public bool Exits(string userId)
        {
            return _userRepository.Exists(userId);
        }

        public PermissionDto? Get(string userId)
        {
           return _userRepository.GetUserRoles(userId);
        }

        public void Permit(PermissionViewModel model)
        {
            _userRepository.Permit(model.UserId, model.Roles);
        }
    }
}
