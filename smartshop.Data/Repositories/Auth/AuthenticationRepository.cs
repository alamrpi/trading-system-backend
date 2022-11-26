using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using smartshop.Common.Helpers;
using smartshop.Data.Configurations;
using smartshop.Data.IRepositories.Auth;
using smartshop.Entities.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace smartshop.Data.Repositories.Auth
{
    internal class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthConfigure _appConfig;

        public AuthenticationRepository(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {
            this._applicationDbContext = applicationDbContext;
            this._userManager = userManager;
            _appConfig = new AuthConfigure();
        }
        public string? GetToken(string userName, string password)
        {
            var user = GetUserByUserName(userName);

            if(user == null)
                return null;

            var isAuthenticate = _userManager.CheckPasswordAsync(user, password).Result;
            if (isAuthenticate)
            {
                return GenerateToken(user.Id, user.UserName, _userManager.GetRolesAsync(user).Result);
            }

            return null;
        }

        public bool UserExistsByEmail(string email) 
            => _applicationDbContext.ApplicationUsers.AsNoTracking().Any(x => x.Email.ToLower() == email.ToLower());

        public bool UserExistsByPhone(string phoneNumber)
             => _applicationDbContext.ApplicationUsers.AsNoTracking().Any(x => x.PhoneNumber == Helper.FilterNumber(phoneNumber));
        
        public ApplicationUser? GetUserByUserName(string userName)
        {
            var user = _userManager.FindByEmailAsync(userName).Result;
            if (user == null)
            {
                if (userName.Contains("@"))
                    return null;

                user = _userManager.FindByNameAsync(Helper.FilterNumber(userName)).Result;
                if (user == null)
                    return null;
            }

            return user;
        }


        private string GenerateToken(string userId, string username, IEnumerable<string> roles)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, username)
            };

            var employee = _applicationDbContext.Employees.Include(x => x.Store).FirstOrDefault(x => x.UserId == userId && x.ResignDate == null);
            if (employee != null)
            {
                claims.Add(new Claim("businessId", employee.Store.BusinessId.ToString()));
                claims.Add(new Claim("storeId", employee.StoreId.ToString()));
            }

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = _appConfig.Issuer,
                Audience = _appConfig.Audience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfig.Token)), SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public void ChangePassword(string userId, string oldPassword, string newPassword)
        {
            var user = _applicationDbContext.ApplicationUsers.FirstOrDefault(x => x.Id == userId);
            if(user != null)
            {
                var result = _userManager.ChangePasswordAsync(user, oldPassword, newPassword).Result;
                if (!result.Succeeded)
                    throw new Exception(result.Errors.FirstOrDefault()?.ToString());

            }
          
        }

        public Employee? GetProfileInfo(string userId)
        {
            return _applicationDbContext.Employees
                .Include(x => x.User)
                .Include(x => x.Designation)
                .ThenInclude(x => x.Business)
                .FirstOrDefault(x => x.UserId == userId && x.ResignDate == null);
        }

        public void UpdateProfile(string userId, ApplicationUser model)
        {
           var user = _applicationDbContext.ApplicationUsers.FirstOrDefault(x => x.Id == userId);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.NormalizedEmail = model.Email.ToUpper();
            user.UserName = model.Email;
            user.NormalizedUserName = model.Email.ToUpper();
            user.PhoneNumber = model.PhoneNumber;
            user.EmailConfirmed = true;
            user.PhoneNumberConfirmed = true;

            _applicationDbContext.Entry(user).State = EntityState.Modified;
            _applicationDbContext.SaveChanges();
        }
    }
}
