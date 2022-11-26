using smartshop.Business.IServices.Additional;
using smartshop.Business.ViewModels.HumanResources;
using smartshop.Entities.Common;
using smartshop.Entities.HumanResource;
using System.Transactions;

namespace smartshop.Business.Service
{
    internal class EmployeeService : IEmployeeService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICloudinaryService _cloudinaryService;

        public EmployeeService(IUserRepository userRepository, IEmployeeRepository employeeRepository, ICloudinaryService cloudinaryService)
        {
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
            this._cloudinaryService = cloudinaryService;
        }
        public bool ChangeStatus(int businessId, int id, bool status) 
            => _employeeRepository.ChangeStatus(businessId, id, status);

        public int Create(EmployeeAddViewModel model)
        {
            using (var ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                if(model.PhotoFile != null)
                {
                    var result = _cloudinaryService.UploadImage(model.PhotoFile.OpenReadStream(), "profile-picture");
                }
                var userId = _userRepository.Create(new ApplicationUser(model.FirstName, model.LastName, model.Email, model.Email, model.PhoneNumber, false));
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User has been not created");

               _ = _employeeRepository.Create(new EmployeePersonalInfo(userId, model.Gender, model.FatherName, model.NationalIdNumber, model.DateOfBirth, model.Address));

                var id = _employeeRepository.Create(new Employee(userId, model.StoreId, model.DesignationId, model.JoiningDate, "N/A", "N/A"));

                ts.Complete();
                return id;
            }
        }

        public int Create(EmployeeViewModel model, string userID)
        {
           return _employeeRepository.Create(new Employee(userID, model.StoreId, model.DesignationId, model.JoiningDate, "N/A", "N/A"));
        }

        public PaginationResponse<EmployeeDto> EmployeeHistories(int businessId, int currentPage, int pageSize, int? storeId = null)
        {
            var employees = _employeeRepository.Get(businessId, currentPage, pageSize, storeId, false);
            return new PaginationResponse<EmployeeDto>
            {
                TotalRows = employees.TotalRows,
                Rows = employees.Rows.Select(e => MapEntityToDto(e))
            };
        }

        public bool Exists(int businessId, int id)
            => _employeeRepository.Exists(businessId, id);

        public PaginationResponse<BaseEmployeeDto> Get(int businessId, int currentPage, int pageSize, int? storeId = null)
        {
            var employees = _employeeRepository.Get(businessId, currentPage, pageSize, storeId);
            return new PaginationResponse<BaseEmployeeDto>
            {
                TotalRows = employees.TotalRows,
                Rows = employees.Rows.Select(e => MapEntityToDto(e))
            };
        }

        public EmployeeDetailsDto? Get(int businessId, int id)
        {
            var employee = _employeeRepository.Get(businessId, id);
            if (employee == null)
                return null;

            var user = employee.User;
            var personalInfo = user.EmployeePersonalInfo;

            return new EmployeeDetailsDto(employee.Id, user.FirstName, user.LastName, employee.Store.Name, employee.Designation.Name, user.Email,
                user.PhoneNumber, user.IsActive, user.PhotoUrl, employee.JoiningDate, employee.ResignDate, personalInfo.Gender, personalInfo.FatherName,
                personalInfo.NationalIdNumber, personalInfo.DateOfBirth, personalInfo.Address)
            {
                UserId = user.Id,
                StoreId = employee.StoreId,
                DesignationId = employee.DesignationId,
                EmployeeHistories = user.Employees.Select(e => new EmployeeHistoryDto()
                {
                    Id = e.Id,
                    Designations = e.Designation.Name,
                    JoiningDate = e.JoiningDate.ToString("dd/MM/yyy"),
                    StoreName = e.Store.Name,
                    ResignDate = e.ResignDate?.ToString("dd/MM/yyy")
                }).ToList()
            };
        }

        public string Update(string userId, EmployeePersonalInfoViewModel model)
        {
            return _employeeRepository.Update(userId, new EmployeePersonalInfo(userId, model.Gender, model.FatherName, model.NationalIdNumber, model.DateOfBirth, model.Address)
            {
                ApplicationUser = new ApplicationUser(model.FirstName, model.LastName, model.Email, model.Email, model.PhoneNumber, false)
            });
        }

        public int Update(int id, EmployeeViewModel model)
        {
            return _employeeRepository.Update(new Employee("", model.StoreId, model.DesignationId, model.JoiningDate, "N/A", "N/A")
            {
                Id = id,
            });
        }

        public bool UserExistsByEmail(string email)
            => _userRepository.UserExistsByEmail(email);

        public bool UserExistsByPhone(string phoneNumber)
            => _userRepository.UserExistsByPhone(phoneNumber);

        private EmployeeDto MapEntityToDto(Employee employee)
        {
            var user = employee.User;
            return new EmployeeDto(employee.Id, user.FirstName, user.LastName, employee.Store.Name, employee.Designation.Name,
                user.Email, user.PhoneNumber, user.PhotoUrl, user.IsActive, employee.JoiningDate, employee.ResignDate)
            { UserId = user.Id};
        }
    }
}
