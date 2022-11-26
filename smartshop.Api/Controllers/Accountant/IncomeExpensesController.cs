using smartshop.Business.Dtos.Accountant;
using smartshop.Business.ViewModels.Accountant;
using smartshop.Common.Constants;
using smartshop.Common.QueryParams.Accountant;

namespace smartshop.Api.Controllers.Accountant
{
    [Route("v{version:apiVersion}/accounting/income-expenses")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.ACCOUNTANT)]
    public class IncomeExpensesController : ControllerBase
    {
        private readonly IIncomeExpenseService _service;
        private readonly ILogger<IncomeExpensesController> _logger;
        private readonly IBankAccountService _bankAccountService;
        private readonly IHeadService _headService;

        public IncomeExpensesController(IIncomeExpenseService service, ILogger<IncomeExpensesController> logger, IBankAccountService bankAccountService, IHeadService headService)
        {
            this._service = service;
            this._logger = logger;
            this._bankAccountService = bankAccountService;
            this._headService = headService;
        }
        [HttpGet("gets")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<PaginationResponse<IncomeExpenseDto>> Get([FromQuery] IncomeExpenseQueryParams queryParams)
        {
            try
            {
                return Ok(_service.Get(User.GetBusinessId(), queryParams));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }
        [HttpGet("gets/ddl-data")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult GetDdlData()
        {
            try
            {
                var businessId = User.GetBusinessId();
                return Ok(new
                {
                    Heads = _headService.Get(businessId),
                    BankAccounts = _bankAccountService.Get(businessId)
                });
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
        public ActionResult<IncomeExpenseDto> Get(int id)
        {
            try
            {
                var incomeExpense = _service.Get(User.GetBusinessId(), id);
                if (incomeExpense == null)
                    return NotFound();

                return Ok(incomeExpense);
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
        public ActionResult Post([FromBody] IncomeExpenseViewModel model)
        {
            try
            {
                var id = _service.Create(model);
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
        public ActionResult Edit([FromBody] IncomeExpenseViewModel model, int id)
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
