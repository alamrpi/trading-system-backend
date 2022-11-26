using smartshop.Business.Dtos.Business;
using smartshop.Business.IServices.Businesses;
using smartshop.Business.ViewModels.Business;
using smartshop.Common.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace smartshop.Api.Controllers.Businesses
{
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StoresController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoresController(IStoreService storeService)
        {
            _storeService = storeService;
        }
        // GET: api/<BusinessesController>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Roles.ADMIN}, {Roles.SUPER_ADMIN}, {Roles.STOCK_INVENTORY}")]
        public ActionResult<PaginationResponse<StoreDto>> Get([FromQuery] PaginateQueryParams queryParams)
        {
            try
            {
                int? businessId = null;
                if (!User.IsInRole("Super Admin"))
                    businessId = User.GetBusinessId();

                return Ok(new PaginationResponse<StoreDto>(_storeService.TotalCount(), _storeService.Gets(queryParams.Page, queryParams.Size, businessId)));

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("for-ddl")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<DropdownDto>> Get([FromQuery]int? businessId)
        {
            try
            {
                if (!User.IsInRole("Super Admin"))
                    businessId = User.GetBusinessId();

                return Ok(_storeService.Gets(businessId));

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<BusinessesController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Roles.ADMIN}, {Roles.SUPER_ADMIN}, {Roles.STOCK_INVENTORY}")]
        public ActionResult<StoreDetailDto> Get(int id)
        {
            try
            {
                var business = _storeService.Get(id);
                if (business == null)
                    return NotFound();

                return Ok(business);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<BusinessesController>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Roles.ADMIN}, {Roles.SUPER_ADMIN}, {Roles.STOCK_INVENTORY}")]
        public ActionResult Post([FromBody] StoreViewModel model)
        {
            try
            {
                if (!User.IsInRole("Super Admin"))
                    model.BusinessId = User.GetBusinessId();

                var id = _storeService.Create(model);
                return CreatedAtAction(nameof(Get), new { Id = id }, model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<BusinessesController>/5
        [HttpPost("{id}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Roles.ADMIN}, {Roles.SUPER_ADMIN}, {Roles.STOCK_INVENTORY}")]
        public ActionResult Put([FromRoute]int id,[FromBody] StoreViewModel model)
        {
            try
            {
                if (!User.IsInRole("Super Admin"))
                    model.BusinessId = User.GetBusinessId();

                if (_storeService.Update(id, model))
                    return AcceptedAtAction(nameof(Get), new { Id = id }, model);

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<BusinessesController>/5
        [HttpPost("{id}/active")]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Roles.ADMIN}, {Roles.SUPER_ADMIN}, {Roles.STOCK_INVENTORY}")]
        public ActionResult Active([FromRoute]int id)
        {
            try
            {
                _storeService.Active(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("{id}/de-active")]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{Roles.ADMIN}, {Roles.SUPER_ADMIN}, {Roles.STOCK_INVENTORY}")]
        public ActionResult DeActive([FromRoute] int id, [FromBody]DeactiveViewModel model)
        {
            try
            {
                _storeService.Deactive(id, model);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
