using smartshop.Business.Dtos.StockInventory;
using smartshop.Common.Constants;

namespace smartshop.Api.Controllers.StockInventory
{
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.STOCK_INVENTORY)]
    public class StocksController : ControllerBase
    {
        private readonly IStockService _service;
        private readonly ILogger<StocksController> _logger;

        public StocksController(IStockService service, ILogger<StocksController> logger)
        {
            this._service = service;
            this._logger = logger;
        }
        [HttpGet("gets")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<PaginationResponse<StockDto>> Get([FromQuery] PaginateQueryParams queryParams, [FromQuery] StockOrDamageQueryParams damageQueryParams)
        {
            try
            {
                return Ok(_service.Gets(User.GetBusinessId(), queryParams.Page, queryParams.Size, damageQueryParams));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        } 
         
        [HttpGet("{storeId}/for-ddl")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<DropdownDto>> GetForDdl([FromRoute] int storeId)
        {
            try
            {
                return Ok(_service.GetForDdl(User.GetBusinessId(), storeId));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }

        [HttpGet("gets-ddl-data")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<StockFilterDdlDto> GetPurchaseDdlData()
        {
            try
            {
                return Ok(_service.GetStockDdlData(User.GetBusinessId()));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }

        [HttpGet("reports")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<PaginationResponse<StockDto>> Get( [FromQuery] StockOrDamageQueryParams damageQueryParams)
        {
            try
            {
                return Ok(_service.GetReports(User.GetBusinessId(), damageQueryParams));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        } 
        
        [HttpGet("damage/gets")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<PaginationResponse<StockDamageDto>> GetDamage([FromQuery] PaginateQueryParams queryParams, [FromQuery] StockOrDamageQueryParams damageQueryParams)
        {
            try
            {
                return Ok(_service.GetDamages(User.GetBusinessId(), queryParams.Page, queryParams.Size, damageQueryParams));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        } 
        
        [HttpGet("damage/reports")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<PaginationResponse<StockDamageDto>> GetDamage([FromQuery] StockOrDamageQueryParams damageQueryParams)
        {
            try
            {
                return Ok(_service.GetDamageReports(User.GetBusinessId(), damageQueryParams));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }

        [HttpPost("damage/create")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult Post([FromBody] StockDamageViewModel model)
        {
            try
            {
                var businessId = User.GetBusinessId();

                if (!_service.ExistsStock(businessId, model.StockId))
                    return BadRequest("Stock id is not valid.");

                var id = _service.CreateDamage(model, User.GetId(), "IP");
                return CreatedAtAction(nameof(Get), new { Id = id }, model);

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
