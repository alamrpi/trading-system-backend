using smartshop.Common.Constants;
using smartshop.Common.QueryParams.Purchase;

namespace smartshop.Api.Controllers.PurchaseManagement
{
    [Route("v{version:apiVersion}/purchase-returns")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PurchaseReturnsController : ControllerBase
    {
        private readonly IPurchaseReturnService _service;
        private readonly ILogger<PurchaseReturnsController> _logger;

        public PurchaseReturnsController(IPurchaseReturnService service, ILogger<PurchaseReturnsController> logger)
        {
            this._service = service;
            this._logger = logger;
        }

        [HttpGet("invoices")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.PURCHASE_MANAGEMENT)]
        public ActionResult<PaginationResponse<PurchaseReturnDto>> Get([FromQuery] PaginateQueryParams queryParams, [FromQuery] PurchaseQueryParams purchaseQuery)
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

        [HttpGet("reports")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.PURCHASE_MANAGEMENT)]
        public ActionResult<PurchaseReturnDto> Get([FromQuery] PurchaseQueryParams purchaseQuery)
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
        public ActionResult<PurchaseReturnDetailsDto> Get(int id)
        {
            try
            {
                var purchaseReturnDto = _service.Get(User.GetBusinessId(), id);
                if (purchaseReturnDto == null)
                    return NotFound();

                return Ok(purchaseReturnDto);
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
        public ActionResult Post([FromBody] PurchaseReturnViewModel model)
        {
            try
            {
                var businessId = User.GetBusinessId();

                var id = _service.Create(model, User.GetId(), "IP");
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
        public ActionResult Edit([FromBody] PurchaseReturnViewModel model, int id)
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.PURCHASE_MANAGEMENT)]
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
