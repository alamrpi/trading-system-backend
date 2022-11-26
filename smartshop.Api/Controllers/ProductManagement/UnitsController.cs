using smartshop.Api.ViewModels;
using smartshop.Business.ViewModels.ProductManagement;
using smartshop.Common.Constants;

namespace smartshop.Api.Controllers.ProductManagement
{
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UnitsController : ControllerBase
    {
        private readonly IUnitService _service;
        private readonly ILogger<UnitsController> _logger;

        public UnitsController(IUnitService service, ILogger<UnitsController> logger)
        {
            this._service = service;
            this._logger = logger;
        }

        [HttpGet("gets")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.PRODUCT_MANAGEMENT)]
        public ActionResult<PaginationResponse<UnitDto>> Get([FromQuery] PaginateQueryParams queryParams)
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

        [HttpGet("get/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.PRODUCT_MANAGEMENT)]
        public ActionResult<UnitDto> Get(int id)
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

        [HttpGet("units-for-ddl")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.PRODUCT_MANAGEMENT)]
        public ActionResult<IEnumerable<DropdownDto>> Get()
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

        [HttpGet("{unitId}/variations-for-ddl")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<DropdownDto>> GetVariations(int unitId)
        {
            try
            {
                return Ok(_service.GetVariations(User.GetBusinessId(), unitId));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }
        
        [HttpGet("{productId}/variations-for-ddl/by-product")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<DropdownDto>> GetVariationByProductId(int productId)
        {
            try
            {
                return Ok(_service.GetVariationsByProductId(User.GetBusinessId(), productId));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        } 
        
        [HttpGet("{stockId}/variations-for-ddl/by-stock")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<DropdownDto>> GetVariationByStockId(int stockId)
        {
            try
            {
                return Ok(_service.GetVariationsByStockId(User.GetBusinessId(), stockId));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }

        [HttpPost("create")]
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.PRODUCT_MANAGEMENT)]
        public ActionResult Post([FromBody] UnitWithVariationViewModel model)
        {
            try
            {
                var businessId = User.GetBusinessId();
                if (_service.Exists(businessId, model.Name))
                    return BadRequest("Unit already exists by this name.");

                var id = _service.Create(businessId, model);
                return CreatedAtAction(nameof(Get), new { Id = id }, model);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("{id}/create-variation")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.PRODUCT_MANAGEMENT)]
        public ActionResult CreateVariation([FromBody] List<UnitVariationViewModel> model, int id)
        {
            try
            {
                var businessId = User.GetBusinessId();

                if (!_service.Exists(businessId, id))
                    return NotFound();

                 _service.CreateUnitVariation(id, model);
                return Ok(model);
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
        public ActionResult Edit([FromBody] UnitViewModel model, int id)
        {
            try
            {
                var businessId = User.GetBusinessId();

                if (!_service.Exists(businessId, id))
                    return NotFound();


                if (_service.Exists(businessId, model.Name, id))
                    return BadRequest("Unit already exists by this name.");

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

        [HttpPost("{id}/delete-variations")]
        [ProducesResponseType(203)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.PRODUCT_MANAGEMENT)]
        public ActionResult DeleteVariations([FromBody] DeleteVariationViewModel model, [FromRoute]int id)
        {
            try
            {
                var businessId = User.GetBusinessId();

                if (!_service.Exists(businessId, id))
                    return NotFound();

                _service.DeleteUnitVariation(model.VariationId);
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
