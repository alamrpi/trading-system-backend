using smartshop.Business.Dtos.Users;
using smartshop.Business.IServices.Auth;
using smartshop.Business.IServices.Users;
using smartshop.Common.Constants;

namespace smartshop.Api.Controllers.BusinessAdmin
{
    [Route("v{version:apiVersion}/business-admin")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.SUPER_ADMIN)]
    public class BusinessAdminsController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IAuthenticationService _authenticationService;

        public BusinessAdminsController(IAdminService adminService, IAuthenticationService authenticationService)
        {
            this._adminService = adminService;
            this._authenticationService = authenticationService;
        }
        // GET: api/<BusinessesController>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<PaginationResponse<ApplicationUserDto>> Get([FromQuery] PaginateQueryParams queryParams)
        {
            try
            {

                return Ok(new PaginationResponse<ApplicationUserDto>
                {
                    Rows = _adminService.Gets(queryParams.Page, queryParams.Size),
                    TotalRows = _adminService.TotalCount()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<BusinessesController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<ApplicationUserDto> Get(string id)
        {
            try
            {
                var business = _adminService.Get(id);
                if (business == null)
                    return NotFound();

                return Ok(business);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<BusinessesController>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        public ActionResult Post([FromBody] ApplicationUserViewModel model)
        {
            try
            {
                if (_authenticationService.UserExistsByEmail(model.Email))
                    return BadRequest("User already exits by that email!");

                if (_authenticationService.UserExistsByPhone(model.PhoneNumber))
                    return BadRequest("User already exits by that phone number!");

                if (_adminService.AnyAdminByBusiness(model.StoreId))
                    return BadRequest("Already have an admin of that business.");

                var userId = _adminService.Create(model);
                return CreatedAtAction(nameof(Get), new { Id = userId }, userId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<BusinessesController>/5
        [HttpPost("{id}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult Put([FromRoute] string id, [FromBody] ApplicationUserViewModel model)
        {
            try
            {
                if (_adminService.Update(id, model))
                    return AcceptedAtAction(nameof(Get), new { Id = id }, model);

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<BusinessesController>/5
        [HttpPost("{id}/active")]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        public ActionResult Active([FromRoute] string id)
        {
            try
            {
                _adminService.Active(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("{id}/de-active")]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        public ActionResult DeActive([FromRoute] string id)
        {
            try
            {
                _adminService.Deactive(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
