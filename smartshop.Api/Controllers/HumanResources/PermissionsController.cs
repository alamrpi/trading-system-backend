using smartshop.Business.IServices.Users;
using smartshop.Business.ViewModels.Users;
using smartshop.Common.Constants;

namespace smartshop.Api.Controllers.HumanResources
{

    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.HR_MANAGEMENT)]
    public class PermissionsController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<PermissionsController> _logger;

        public PermissionsController(IUserService userService, ILogger<PermissionsController> logger)
        {
            this._userService = userService;
            this._logger = logger;
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<PermissionDto> Get(string userId)
        {
            try
            {
                var roles = _userService.Get(userId);
                if (roles == null)
                    return NotFound();

                return Ok(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }

        [HttpPost("permit")]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        public ActionResult Post([FromBody] PermissionViewModel model)
        {
            try
            {

                if (!_userService.Exits(model.UserId))
                    return NotFound();

                 _userService.Permit(model);
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }

    }
}
