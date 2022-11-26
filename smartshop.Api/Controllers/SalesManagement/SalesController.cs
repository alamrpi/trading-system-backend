using smartshop.Business.Dtos.SalesManagement;
using smartshop.Common.Constants;

namespace smartshop.Api.Controllers.SalesManagement
{

    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _service;
        private readonly ILogger<SalesController> _logger;

        public SalesController(ISaleService service, ILogger<SalesController> logger)
        {
            this._service = service;
            this._logger = logger;
        }


        [HttpGet("invoices")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.SALES_MANAGEMENT)]
        public ActionResult<PaginationResponse<SaleDto>> Get([FromQuery] PaginateQueryParams queryParams, [FromQuery] SalesQueryParams purchaseQuery)
        {
            try
            {
                return Ok(_service.Get(User.GetBusinessId(), queryParams.Page, queryParams.Size, purchaseQuery));
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
        public ActionResult<DropdownDto> Get(int? storeId, int? customerId)
        {
            try
            {
                return Ok(_service.Get(User.GetBusinessId(), storeId, customerId));
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.SALES_MANAGEMENT)]
        public ActionResult<SalesDdlDto> GetPurchaseDdlData()
        {
            try
            {
                return Ok(_service.GetDdlData(User.GetBusinessId()));
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.SALES_MANAGEMENT)]
        public ActionResult<PurchaseDto> Get([FromQuery] SalesQueryParams purchaseQuery)
        {
            try
            {
                return Ok(_service.Get(User.GetBusinessId(), purchaseQuery));
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.SALES_MANAGEMENT)]
        public ActionResult<SaleDetailsDto> Get(int id)
        {
            try
            {
                var saleDto = _service.Get(User.GetBusinessId(), id);
                if (saleDto == null)
                    return NotFound();

                return Ok(saleDto);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }

        [HttpGet("get/{id}/sale-products")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.SALES_MANAGEMENT)]
        public ActionResult<IEnumerable<SaleProductDto>> GetSaleProducts(int id)
        {
            try
            {
                var purchase = _service.GetSaleProducts(User.GetBusinessId(), id);
                if (purchase == null)
                    return NotFound();

                return Ok(purchase);
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.SALES_MANAGEMENT)]
        public ActionResult Post([FromBody] SaleViewModel model)
        {
            try
            {
                var businessId = User.GetBusinessId();

                var id = _service.Create(model, User.GetId(), "IP", businessId);
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.SALES_MANAGEMENT)]
        public ActionResult Edit([FromBody] SaleViewModel model, int id)
        {
            try
            {
                var businessId = User.GetBusinessId();

                if (!_service.Exists(businessId, id))
                    return NotFound();


                if (_service.AnyDueCollectionOrReturn(businessId, id))
                    return BadRequest("Some operation are occure on that invoice. Can't update this invoice.");

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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.SALES_MANAGEMENT)]
        public ActionResult Delete(int id)
        {
            try
            {
                var businessId = User.GetBusinessId();

                if (!_service.Exists(businessId, id))
                    return NotFound();


                if (_service.AnyDueCollectionOrReturn(businessId, id))
                    return BadRequest("Some operation are occure on that invoice. Can't update this invoice.");


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
