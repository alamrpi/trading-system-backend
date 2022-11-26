using smartshop.Business.Dtos.PurchaseManagement;
using smartshop.Common.Constants;
using smartshop.Common.QueryParams.Purchase;

namespace smartshop.Api.Controllers.PurchaseManagement
{

    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchaseService _service;
        private readonly ILogger<PurchasesController> _logger;

        public PurchasesController(IPurchaseService service, ILogger<PurchasesController> logger)
        {
            this._service = service;
            this._logger = logger;
        }

        [HttpGet("invoices")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.PURCHASE_MANAGEMENT)]
        public ActionResult<PaginationResponse<PurchaseDto>> Get([FromQuery] PaginateQueryParams queryParams, [FromQuery]PurchaseQueryParams purchaseQuery)
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
        public ActionResult<DropdownDto> Get(int? storeId, int? supplierId)
        {
            try
            {
                return Ok(_service.Get(User.GetBusinessId(), storeId, supplierId));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        } 
        
        [HttpGet("gets-purchase-ddl-data")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.PURCHASE_MANAGEMENT)]
        public ActionResult<PurchaseDdlDto> GetPurchaseDdlData()
        {
            try
            {
                return Ok(_service.GetPurchaseDdl(User.GetBusinessId()));
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.PURCHASE_MANAGEMENT)]
        public ActionResult<PurchaseDto> Get([FromQuery] PurchaseQueryParams purchaseQuery)
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.PURCHASE_MANAGEMENT)]
        public ActionResult<PurchaseDetailsDto> Get(int id)
        {
            try
            {
                var purchase = _service.Get(User.GetBusinessId(), id);
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
        
        [HttpGet("get/{id}/purchase-products")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.PURCHASE_MANAGEMENT)]
        public ActionResult<IEnumerable<PurchaseProductDto>> GetPurchaseProducts(int id)
        {
            try
            {
                var purchase = _service.GetPurchaseProducts(User.GetBusinessId(), id);
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.PURCHASE_MANAGEMENT)]
        public ActionResult Post([FromBody] PurchaseViewModel model)
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.PURCHASE_MANAGEMENT)]
        public ActionResult Edit([FromBody] PurchaseViewModel model, int id)
        {
            try
            {
                var businessId = User.GetBusinessId();

                if (!_service.Exists(businessId, id))
                    return NotFound();


                if (_service.AnyDuePaymentOrReturn(businessId, id))
                    return BadRequest("There are operation occure on that invoice.");

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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.PURCHASE_MANAGEMENT)]
        public ActionResult Delete(int id)
        {
            try
            {
                var businessId = User.GetBusinessId();

                if (!_service.Exists(businessId, id))
                    return NotFound();


                if (_service.AnyDuePaymentOrReturn(businessId, id))
                    return BadRequest("There are operation occure on that invoice.");


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
