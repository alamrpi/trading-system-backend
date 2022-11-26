using smartshop.Business.IServices.Businesses;
using smartshop.Business.ViewModels.HumanResources;
using smartshop.Common.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace smartshop.Api.Controllers.HumanResources
{
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthPolicies.HR_MANAGEMENT)]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeesController> _logger;
        private readonly IStoreService _storeService;
        private readonly IDesignationService _designationService;

        public EmployeesController(IEmployeeService employeeService, ILogger<EmployeesController> logger, IStoreService storeService, IDesignationService designationService)
        {
            this._employeeService = employeeService;
            this._logger = logger;
            this._storeService = storeService;
            this._designationService = designationService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<PaginationResponse<BaseEmployeeDto>> Get([FromQuery] EmployeeQueryParams queryParams)
        {
            try
            {
                return Ok(_employeeService.Get(User.GetBusinessId(), queryParams.Page, queryParams.Size, queryParams.StoreId));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }

        [HttpGet("get-histories")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<PaginationResponse<EmployeeDto>> GetEmployees([FromQuery] EmployeeQueryParams queryParams)
        {
            try
            {
                return Ok(_employeeService.EmployeeHistories(User.GetBusinessId(), queryParams.Page, queryParams.Size, queryParams.StoreId));
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }

        [HttpGet("{id}/get-details")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<EmployeeDetailsDto> GetDetails([FromRoute]int id)
        {
            try
            {
                var employee = _employeeService.Get(User.GetBusinessId(), id);
                if (employee == null)
                    return NotFound();

                return Ok(employee);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }

        [HttpPost("create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<EmployeeDetailsDto> Create([FromForm] EmployeeAddViewModel model)
        {
            try
            {
                if (_employeeService.UserExistsByEmail(model.Email))
                    return BadRequest("Already have an account by this Email.");

                if (_employeeService.UserExistsByPhone(model.PhoneNumber))
                    return BadRequest("Already have an account by this Phone.");

                var id = _employeeService.Create(model);

                return CreatedAtAction(nameof(GetDetails), new {id}, id);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }

        [HttpPost("{userId}/transfer-or-promotion")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<EmployeeDetailsDto> Create([FromBody] EmployeeViewModel model, [FromRoute]string userId)
        {
            try
            {
                var employeeId = _employeeService.Create(model, userId);

                return CreatedAtAction(nameof(GetDetails), new { id = employeeId }, employeeId); ;
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }

        [HttpPost("{id}/edit-employee-info")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<EmployeeDetailsDto> Update([FromBody] EmployeeViewModel model, int id)
        {
            try
            {
                if (!_employeeService.Exists(User.GetBusinessId(), id))
                    return NotFound();

                var employeeId = _employeeService.Update(id, model);

                return AcceptedAtAction(nameof(GetDetails), new { id }, employeeId); ;
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }

        [HttpPost("{userId}/edit-user-info")]
        [ProducesResponseType(202)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<EmployeeDetailsDto> Update([FromBody] EmployeePersonalInfoViewModel model, string userId)
        {
            try
            {
                //if (_employeeService.Exists(User.GetBusinessId(), userId))
                //    return NotFound();

                var employeeId = _employeeService.Update(userId, model);

                return AcceptedAtAction(nameof(GetDetails), new { id = employeeId }, employeeId); ;
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }

        [HttpPost("{id}/active")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<EmployeeDetailsDto> Active(int id)
        {
            try
            {
                if (!_employeeService.Exists(User.GetBusinessId(), id))
                    return NotFound();

                var employeeId = _employeeService.ChangeStatus(User.GetBusinessId(), id, true);

                return Ok(employeeId); ;
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        }

        [HttpPost("{id}/de-active")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<EmployeeDetailsDto> DeActive(int id)
        {
            try
            {
                if (!_employeeService.Exists(User.GetBusinessId(), id))
                    return NotFound();

                var employeeId = _employeeService.ChangeStatus(User.GetBusinessId(), id, false);

                return Ok(employeeId); ;
            }
            catch (Exception ex)
            {
                LogError(ex);
                return StatusCode(500, "Something went wrong. Contact support team.");
            }
        } 
        
        [HttpGet("get-dropdown-data")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult GetDropdownData()
        {
            try
            {
                var businessId = User.GetBusinessId();

                return Ok(new
                {
                    Stores = _storeService.Gets(businessId),
                    Designations = _designationService.Get(businessId)
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
