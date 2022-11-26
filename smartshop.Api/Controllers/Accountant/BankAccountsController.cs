using smartshop.Common.Constants;

namespace smartshop.Api.Controllers.Accountant
{
    [Route("v{version:apiVersion}/accounting/bank-accounts")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BankAccountsController : ControllerBase
    {
        private readonly IBankAccountService _service;
        private readonly ILogger<BankAccountsController> _logger;

        public BankAccountsController(IBankAccountService service, ILogger<BankAccountsController> logger)
        {
            this._service = service;
            this._logger = logger;
        }

        [HttpGet("gets")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.ACCOUNTANT)]
        public ActionResult<PaginationResponse<BankAccountDto>> Get([FromQuery] PaginateQueryParams queryParams)
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
        public ActionResult<DropdownDto> Get()
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.ACCOUNTANT)]
        public ActionResult<BankAccountDto> Get(int id)
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.ACCOUNTANT)]
        public ActionResult Post([FromBody] BankAccountViewModel model)
        {
            try
            {
                var businessId = User.GetBusinessId();

                if (_service.Exists(businessId, model.Name))
                    return BadRequest("Bank account exists by this name.");

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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.ACCOUNTANT)]
        public ActionResult Edit([FromBody] BankAccountViewModel model, int id)
        {
            try
            {
                var businessId = User.GetBusinessId();

                if (!_service.Exists(businessId, id))
                    return NotFound();


                if (_service.Exists(businessId, model.Name, id))
                    return BadRequest("Bank account already exists by this name.");

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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.ACCOUNTANT)]
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
