// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

using smartshop.Common.Constants;

namespace smartshop.Api.Controllers.HumanResources
{
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.HR_MANAGEMENT)]
    public class DesignationsController : ControllerBase
    {
        private readonly IDesignationService _designationService;

        public DesignationsController(IDesignationService designationService)
        {
            _designationService = designationService;
        }

        // GET: api/<DesignationsController>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<PaginationResponse<DesignationDto>> Get([FromQuery] PaginateQueryParams queryParams)
        {
            try
            {
                return Ok(_designationService.Get(User.GetBusinessId(), queryParams.Page, queryParams.Size));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }

        // GET api/<DesignationsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<DesignationDto> Get(int id)
        {
            try
            {
                var designation = _designationService.Get(User.GetBusinessId(), id);
                if (designation == null)
                    return NotFound();

                return Ok(designation);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }

        // GET api/<DesignationsController>/for-ddl
        [HttpGet("for-ddl")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<DropdownDto>> Get()
        {
            try
            {
                return Ok(_designationService.Get(User.GetBusinessId()));

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
        public ActionResult Post([FromBody] DesignationViewModel model)
        {
            try
            {
                var businessId = User.GetBusinessId();

                if (_designationService.Exits(businessId, model.Name))
                    return BadRequest("Already have a designation by this name");

                var id = _designationService.CreateOrUpdate(businessId, model);
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
        public ActionResult Post([FromRoute] int id, [FromBody] DesignationViewModel model)
        {
            try
            {
                var businessId = User.GetBusinessId();
                if (!_designationService.Exits(businessId, id))
                    return NotFound("Data Not found!");

                if (_designationService.Exits(businessId, model.Name, id))
                    return BadRequest("Already have a designation by this name");

                var result = _designationService.CreateOrUpdate(businessId, model, id);

                return CreatedAtAction(nameof(Get), new { Id = result }, model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<DesignationsController>/5
        [HttpPost("{id}/delete")]
        [ProducesResponseType(203)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult Delete(int id)
        {
            var businessId = User.GetBusinessId();
            if (!_designationService.Exits(businessId, id))
                return NotFound("Data Not found!");

            _designationService.Delete(businessId, id);

            return NoContent();
        }


        private void LogError(Exception ex)
        {
            //_logger.LogError(ex, ex.Message);
        }
    }
}
