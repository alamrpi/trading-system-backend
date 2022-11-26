using smartshop.Common.Constants;
using smartshop.Common.Enums.Accounting;
using smartshop.Common.QueryParams.Accountant;

namespace smartshop.Api.Controllers.Accountant
{
    [Route("v{version:apiVersion}/accounting/statements")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.ACCOUNTANT)]
    public class StatementsController : ControllerBase
    {
        private readonly ILogger<StatementsController> _logger;
        private readonly IAccountTransactionService _accountTransactionService;

        public StatementsController(ILogger<StatementsController> logger, IAccountTransactionService accountTransactionService)
        {
            this._logger = logger;
            this._accountTransactionService = accountTransactionService;
        }

        [HttpGet("owner-equity")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult OwnerEquity([FromQuery]StatementQueryParams queryParams)
        {
            try
            {
                var statements = _accountTransactionService.GetTransactionByComponent(EquationComponent.OwnersEquity, User.GetBusinessId(), queryParams);
                return Ok(new
                {
                    Rows = statements,
                    TotalPlus = statements.Where(x => x.Operator == Operators.PLUS).Select(x => x.Amount).Sum(),
                    TotalMinus = statements.Where(x => x.Operator == Operators.MINUS).Select(x => x.Amount).Sum(),
                });
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }
       
        [HttpGet("balance-sheet")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult BalanceSheet([FromQuery] StatementQueryParams queryParams)
        {
            try
            {
                var statements = _accountTransactionService.GetBalanceSheet(User.GetBusinessId(), queryParams);
                var totalAssetsPlus = statements.Where(x => x.Component == EquationComponent.Assets).Where(x => x.Operator == Operators.PLUS).Select(x => x.Amount).Sum();
                var totalAssetsMinus = statements.Where(x => x.Component == EquationComponent.Assets).Where(x => x.Operator == Operators.MINUS).Select(x => x.Amount).Sum();
                var totalLiOwPlus = statements.Where(x => x.Component != EquationComponent.Assets).Where(x => x.Operator == Operators.PLUS).Select(x => x.Amount).Sum();
                var totalLiOwMinus = statements.Where(x => x.Component != EquationComponent.Assets).Where(x => x.Operator == Operators.MINUS).Select(x => x.Amount).Sum();
                return Ok(new
                {
                    Rows = statements,
                    BalanceAssets = totalAssetsPlus - totalAssetsMinus,
                    BalanceLiabilitiesOwners = totalLiOwPlus - totalLiOwMinus,
                });
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }
        [HttpGet("cash-flows")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult CashFlows([FromQuery] StatementQueryParams queryParams)
        {
            try
            {
                var statements = _accountTransactionService.GetTransactionByAccount(TransactionAccount.CASH, User.GetBusinessId(), queryParams);
                return Ok(new
                {
                    Rows = statements,
                    TotalPlus = statements.Where(x => x.Operator == Operators.PLUS).Select(x => x.Amount).Sum(),
                    TotalMinus = statements.Where(x => x.Operator == Operators.MINUS).Select(x => x.Amount).Sum(),
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
