using Microsoft.AspNetCore.Identity;
using smartshop.Common.Helpers;
using smartshop.Entities.Common;

namespace smartshop.Data.Repositories.Users
{
    internal class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepository(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this._applicationDbContext = applicationDbContext;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }
        public string? Create(ApplicationUser applicationUser)
        {
            var result = _userManager.CreateAsync(applicationUser, "User123!").Result;

            if (result.Succeeded)
            {
                _ = _userManager.AddToRoleAsync(applicationUser, "User");
                return applicationUser.Id;
            }
            return null;
        }

        public bool Exists(string userId) 
            => _applicationDbContext.ApplicationUsers.AsNoTracking().Any(x => x.Id == userId);

        public PermissionDto? GetUserRoles(string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null)
                return null;

            var userRoles = _roleManager.Roles.ToList();

            return new PermissionDto()
            {
                Id = user.Id,
                Name = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                PicturePath = user.PhotoUrl,
                Roles = userRoles.Select(r => new UserRolesDto
                {
                  RoleName = r.Name,
                  IsSet = _userManager.IsInRoleAsync(user, r.Name).Result
                }).ToList()
            };
        }

        public void Permit(string userId, List<string> roles)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            var result = _userManager.AddToRolesAsync(user, roles).Result;
            if(!result.Succeeded)
                throw new Exception(result.Errors.ToString());
        }

        public bool UserExistsByEmail(string email)
           => _applicationDbContext.ApplicationUsers.AsNoTracking().Any(x => x.Email.ToLower() == email.ToLower());

        public bool UserExistsByPhone(string phoneNumber)
             => _applicationDbContext.ApplicationUsers.AsNoTracking().Any(x => x.PhoneNumber == Helper.FilterNumber(phoneNumber));
    }
}
