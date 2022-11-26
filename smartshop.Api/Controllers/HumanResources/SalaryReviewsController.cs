using smartshop.Business.IServices.HumanResources;
using smartshop.Business.ViewModels.HumanResources;
using smartshop.Common.Constants;

namespace smartshop.Api.Controllers
{
    [Route("v{version:apiVersion}/salary-reviews")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.HR_MANAGEMENT)]
    public class SalaryReviewsController : ControllerBase
    {
        private readonly ISalaryReviewService _salaryReviewService;
        private readonly ILogger<SalaryReviewsController> _logger;
        private readonly IEmployeeService _employeeService;

        public SalaryReviewsController(ISalaryReviewService salaryReviewService, ILogger<SalaryReviewsController> logger, IEmployeeService employeeService)
        {
            this._salaryReviewService = salaryReviewService;
            this._logger = logger;
            this._employeeService = employeeService;
        }

        [HttpGet("gets")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<PaginationResponse<SalaryReviewDto>> Get([FromQuery] EmployeeQueryParams queryParams)
        {
            try
            {
                return Ok(_salaryReviewService.Get(User.GetBusinessId(), queryParams.Page, queryParams.Size, queryParams.StoreId));
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
        public ActionResult<SalaryReviewDto> Get(int id)
        {
            try
            {
                var salaryReview = _salaryReviewService.Get(User.GetBusinessId(), id);
                if (salaryReview == null)
                    return NotFound();

                return Ok(salaryReview);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }

        [HttpGet("get-by/{employeeId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<SalaryReviewDto> GetByEmployeeId(int employeeId)
        {
            try
            {
                var salaryReview = _salaryReviewService.GetByEmployeeId(User.GetBusinessId(), employeeId);
                if (salaryReview == null)
                    return Ok();

                return Ok(salaryReview);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }

        [HttpPost("{employeeId}/create")]
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult Post([FromBody] SalaryReviewViewModel model, int employeeId)
        {
            try
            {
                var businessId = User.GetBusinessId();

                if (!_employeeService.Exists(businessId, employeeId))
                    return NotFound();

                var id = _salaryReviewService.Create(businessId, model);
                return CreatedAtAction(nameof(Get), new { Id = id }, model);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("{id}/edit")]
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult Edit([FromBody] SalaryReviewViewModel model, int id)
        {
            try
            {
                var businessId = User.GetBusinessId();

                if (!_salaryReviewService.Exists(businessId, id))
                    return NotFound();

                var result = _salaryReviewService.Update(businessId, id, model);
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
