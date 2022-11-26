using smartshop.Common.Constants;

namespace smartshop.Api.Controllers.Accountant
{
    [Route("v{version:apiVersion}/accounting/investors")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.ACCOUNTANT)]
    public class InvestorsController : ControllerBase
    {
        private readonly IInvestorService _service;
        private readonly ILogger<InvestorsController> _logger;

        public InvestorsController(IInvestorService service, ILogger<InvestorsController> logger)
        {
            this._service = service;
            this._logger = logger;
        }

        [HttpGet("gets")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<PaginationResponse<InvestorDto>> Get([FromQuery] PaginateQueryParams queryParams)
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
        [HttpGet("gets-for-ddl")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IList<DropdownDto>> GetForDdl()
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

        [HttpGet("get/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<InvestorDto> Get(int id)
        {
            try
            {
                var unitDto = _service.Get(User.GetBusinessId(), id);
                if (unitDto == null)
                    return NotFound();

                return Ok(unitDto);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }

        [HttpPost("create")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult Post([FromBody] InvestorViewModel model)
        {
            try
            {
                var businessId = User.GetBusinessId();

                var id = _service.Create(businessId, model);
                return CreatedAtAction(nameof(Get), new { Id = id }, model);

            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost("{id}/edit")]
        [ProducesResponseType(202)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult Edit([FromBody] InvestorViewModel model, int id)
        {
            try
            {
                var businessId = User.GetBusinessId();

                if (!_service.Exists(businessId, id))
                    return NotFound();

                _service.Update(id, model);
                return AcceptedAtAction(nameof(Get), new { Id = id }, model);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("{id}/delete")]
        [ProducesResponseType(203)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult Delete(int id)
        {
            try
            {
                var businessId = User.GetBusinessId();

                if (!_service.Exists(businessId, id))
                    return NotFound();

                _service.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, ex.Message);
            }
        }

        private void LogError(Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
