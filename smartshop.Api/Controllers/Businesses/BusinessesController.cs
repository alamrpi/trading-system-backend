using smartshop.Business.Dtos.Business;
using smartshop.Business.IServices.Businesses;
using smartshop.Business.ViewModels.Business;
using smartshop.Common.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace smartshop.Api.Controllers.Businesses
{
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BusinessesController : ControllerBase
    {
        private readonly IBusinessService _businessService;
        private readonly ILogger<BusinessesController> _logger;

        public BusinessesController(IBusinessService businessService, ILogger<BusinessesController> logger)
        {
            _businessService = businessService;
            this._logger = logger;
        }
        // GET: api/<BusinessesController>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.SUPER_ADMIN)]
        public ActionResult<PaginationResponse<BusinessDto>> Get([FromQuery] PaginateQueryParams queryParams)
        {
            try
            {
                var business = _businessService.Gets(queryParams.Page, queryParams.Size).ToList();
                return Ok(new PaginationResponse<BusinessDto>()
                {
                    Rows = business,
                    TotalRows = _businessService.TotalCount()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }

        // GET api/<BusinessesController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.SUPER_ADMIN)]
        public ActionResult<BusinessDetailsDto> Get(int id)
        {
            try
            {
                var business = _businessService.Get(id);
                if (business == null)
                    return NotFound();

                return Ok(business);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("for-ddl")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.SUPER_ADMIN)]
        public ActionResult<IEnumerable<DropdownDto>> Get()
        {
            try
            {
                return Ok(_businessService.Gets());

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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.SUPER_ADMIN)]
        public ActionResult Post([FromForm] BusinessViewModel model)
        {
            try
            {
                var id = _businessService.Create(model);
                return CreatedAtAction(nameof(Get), new { Id = id }, model);
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.SUPER_ADMIN)]
        public ActionResult Edit([FromRoute]int id,[FromForm] BusinessViewModel model)
        {
            try
            {
                if(_businessService.Update(id, model))
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.SUPER_ADMIN)]
        public ActionResult Active([FromRoute]int id)
        {
            try
            {
                _businessService.Active(id);
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.SUPER_ADMIN)]
        public ActionResult DeActive([FromRoute] int id, [FromBody]DeactiveViewModel model)
        {
            try
            {
                _businessService.Deactive(id, model);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("save-changes")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.APP_SETTING)]
        public ActionResult<BusinessDetailsDto> GetForSaveChange()
        {
            try
            {
                var business = _businessService.Get(User.GetBusinessId());
                if (business == null)
                    return NotFound();

                return Ok(business);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("save-changes")]
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.APP_SETTING)]
        public ActionResult SaveChange([FromForm] BusinessViewModel model)
        {
            try
            {
                var businessId = User.GetBusinessId();

                if (_businessService.Update(businessId, model))
                    return AcceptedAtAction(nameof(Get), new { Id = businessId }, model);

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("basic-configure")]
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.APP_SETTING)]
        public ActionResult UpdateBasicConfigure([FromBody] BusinessConfigureViewModel model)
        {
            try
            {
                if (_businessService.Update(User.GetBusinessId(), model))
                    return AcceptedAtAction(nameof(Get), new { Id = User.GetBusinessId() }, model);

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("basic-configure")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.APP_SETTING)]
        public ActionResult<BusinessConfigureDto> GetConfigure()
        {
            try
            {
                var business = _businessService.GetBusinessConfigure(User.GetBusinessId());
                if (business == null)
                    return Ok();

                return Ok(business);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
