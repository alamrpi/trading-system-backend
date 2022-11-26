using smartshop.Common.Constants;

namespace smartshop.Api.Controllers
{
    [Route("v{version:apiVersion}/accounting/heads")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HeadsController : ControllerBase
    {
        private readonly IHeadService _service;
        private readonly ILogger<HeadsController> _logger;
        public HeadsController(IHeadService service, ILogger<HeadsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: api/<HeadsController>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.ACCOUNTANT)]
        public ActionResult<PaginationResponse<HeadDto>> Get([FromQuery] PaginateQueryParams queryParams)
        {
            try
            {
                return Ok(_service.Get(User.GetBusinessId(), queryParams.Page, queryParams.Size));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }

        // GET api/<HeadsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.ACCOUNTANT)]
        public ActionResult<HeadDto> Get(int id)
        {
            try
            {
                var head = _service.Get(User.GetBusinessId(), id);
                if (head == null)
                    return NotFound();

                return Ok(head);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }

        // GET api/<HeadsController>/for-ddl
        [HttpGet("for-ddl")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<DropdownDto>> Get()
        {
            try
            {
                return Ok(_service.Get(User.GetBusinessId()));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }


        // POST api/<DesignationsController>
        [HttpPost("create")]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.ACCOUNTANT)]
        public ActionResult Post([FromBody] HeadViewModel model)
        {
            try
            {
                var businessId = User.GetBusinessId();

                if (_service.Exits(businessId, model.Name))
                    return BadRequest("Already have a head by this name");

                var id = _service.CreateOrUpdate(businessId, model);
                return CreatedAtAction(nameof(Get), new { Id = id }, model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<DesignationsController>/5
        [HttpPost("{id}/edit")]
        [ProducesResponseType(202)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.ACCOUNTANT)]
        public ActionResult Post([FromRoute] int id, [FromBody] HeadViewModel model)
        {
            try
            {
                var businessId = User.GetBusinessId();
                if (!_service.Exits(businessId, id))
                    return NotFound("Data Not found!");

                if (_service.Exits(businessId, model.Name, id))
                    return BadRequest("Already have a head by this name");

                var result = _service.CreateOrUpdate(businessId, model, id);

                return CreatedAtAction(nameof(Get), new { Id = result }, model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        // DELETE api/<HeadsController>/5
        [HttpPost("{id}/delete")]
        [ProducesResponseType(203)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.ACCOUNTANT)]
        public ActionResult Delete(int id)
        {
            var businessId = User.GetBusinessId();

            if (!_service.Exits(businessId, id))
                return NotFound("Data Not found!");

            var head = _service.Get(businessId, id);
            if (head.IsAnyTransaction)
                return BadRequest("There have transactions available");
          
            _service.Delete(businessId, id);

            return NoContent();
        }


        private void LogError(Exception ex) 
            => _logger.LogError(ex, ex.Message);
    }
}
