using smartshop.Common.Constants;
using smartshop.Common.Enums.Accounting;

namespace smartshop.Api.Controllers.Accountant
{
    [Route("v{version:apiVersion}/accounting/ledgers")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.ACCOUNTANT)]
    public class LedgersController : ControllerBase
    {
        private readonly IHeadTransactionService _service;
        private readonly ILogger<LedgersController> _logger;
        public LedgersController(IHeadTransactionService service, ILogger<LedgersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // GET: api/<HeadsController>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<LedgerDto>> Get([FromQuery] LedgerFilterQueryParams queryParams)
        {
            try
            {
                var ledgers = _service.GetLedgers(queryParams);
                return Ok(new
                {
                    Rows = ledgers,
                    TotalCredit = ledgers.Where(x => x.Type == TransactionType.Credit).Select(x => x.Amount).Sum(),
                    TotalDebit = ledgers.Where(x => x.Type == TransactionType.Debit).Select(x => x.Amount).Sum(),
                });
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }

        private void LogError(Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
