using smartshop.Business.ViewModels.ProductManagement;
using smartshop.Common.Constants;

namespace smartshop.Api.Controllers.ProductManagement
{
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService service, ILogger<ProductsController> logger)
        {
            this._service = service;
            this._logger = logger;
        }

        [HttpGet("gets")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.PRODUCT_MANAGEMENT)]
        public ActionResult<PaginationResponse<ProductDto>> Get([FromQuery] PaginateQueryParams queryParams,[FromQuery] ProductQueryParams productQuery)
        {
            try
            {
                return Ok(_service.Get(User.GetBusinessId(), queryParams.Page, queryParams.Size, productQuery));
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
        public ActionResult<DropdownDto> GetForDdl([FromQuery] ProductQueryParams param)
        {
            try
            {
                return Ok(_service.Get(User.GetBusinessId(), param));
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.PRODUCT_MANAGEMENT)]
        public ActionResult<DropdownDataForProductViewModel> GetDdlData()
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

        [HttpGet("get/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.PRODUCT_MANAGEMENT)]
        public ActionResult<ProductDto> Get(int id)
        {
            try
            {
                var category = _service.Get(User.GetBusinessId(), id);
                if (category == null)
                    return NotFound();

                return Ok(category);
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.PRODUCT_MANAGEMENT)]
        public ActionResult Post([FromBody] ProductViewModel model)
        {
            try
            {
                var businessId = User.GetBusinessId();

                //if (_service.Exists(businessId, model.Name))
                //    return BadRequest("Category already exists by this name.");

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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.PRODUCT_MANAGEMENT)]
        public ActionResult Edit([FromBody] ProductViewModel model, int id)
        {
            try
            {
                var businessId = User.GetBusinessId();

                if (!_service.Exists(businessId, id))
                    return NotFound();


                //if (_service.Exists(businessId, model.Name, id))
                //    return BadRequest("Category already exists by this name.");

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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.PRODUCT_MANAGEMENT)]
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
