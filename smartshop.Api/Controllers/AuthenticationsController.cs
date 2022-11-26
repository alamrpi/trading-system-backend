using smartshop.Api.ViewModels;
using smartshop.Business.Dtos.Users;
using smartshop.Business.IServices.Auth;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace smartshop.Api.Controllers
{
    [Route("v{version:apiVersion}/common/[controller]")]
    [ApiController]
    public class AuthenticationsController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationsController(IAuthenticationService authenticationService)
        {
            this._authenticationService = authenticationService;
        }

        // GET: api/<AuthController>
        [HttpPost("get-token")]
        public ActionResult<string> Get([FromBody]AuthenticationViewModel model)
        {
            var token = _authenticationService.GetToken(model.UserName, model.Password);
            if (token == null)
                return BadRequest("User name or password incorrect.");

            return Ok(token);
        }

        [HttpPost("change-password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            try
            {
                _authenticationService.ChangePassword(User.GetId(), model.OldPassword, model.NewPassword);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/<AuthController>
        [HttpGet("get-profile-info")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<ProfileInfoDto> GetProfileInfo()
        {
            var userId = User.GetId();
            var info = _authenticationService.GetProfileInfo(userId);
            return Ok(info);
        }

        [HttpPost("update-profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult UpdateProfile([FromForm] UpdateProfileViewModel model)
        {
            try
            {
                _authenticationService.UpdateProfile(User.GetId(), model);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
