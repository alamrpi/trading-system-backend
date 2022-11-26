using smartshop.Business.IServices.HumanResources;
using smartshop.Business.ViewModels.HumanResources;
using smartshop.Entities.HumanResource;

namespace smartshop.Business.Service.HumanResources
{
    internal class SalaryReviewService : ISalaryReviewService
    {
        private readonly ISalaryReviewRepository _repository;

        public SalaryReviewService(ISalaryReviewRepository repository)
        {
            this._repository = repository;
        }
        int ISalaryReviewService.Create(int employeeId, SalaryReviewViewModel model) 
            => _repository.Create(new EmployeeSalaryReview(employeeId, model.BasicSalary, model.HouseRent, model.TransportAllowance, model.MedicalAllowance, model.MealAllowance, model.ProvidenceFund, model.Insurance, model.Tax, model.StartDate));

        PaginationResponse<SalaryReviewDto> ISalaryReviewService.Get(int businessId, int currentPage, int pageSize, int? storeId)
        {
            var reviews = _repository.Get(businessId, currentPage, pageSize, storeId);

            return new PaginationResponse<SalaryReviewDto>
            {
                TotalRows = reviews.TotalRows,
                Rows = reviews.Rows.Select(s => MapEntityToDto(s)).ToList(),
            };

        }

        SalaryReviewDto? ISalaryReviewService.Get(int businessId, int id)
        {
            var salaryReview = _repository.Get(businessId, id);
            if (salaryReview == null)
                return null;

            return MapEntityToDto(salaryReview);
        }

        SalaryReviewDto? ISalaryReviewService.GetByEmployeeId(int businessId, int employeeId)
        {
            var salaryReview = _repository.GetByEmployeeId(businessId, employeeId);
            if (salaryReview == null)
                return null;

            return MapEntityToDto(salaryReview);
        }

        int ISalaryReviewService.Update(int businessId, int id, SalaryReviewViewModel model)
            => _repository.Update(businessId, id, new EmployeeSalaryReview(0, model.BasicSalary, model.HouseRent, model.TransportAllowance, model.MedicalAllowance, model.MealAllowance, model.ProvidenceFund, model.Insurance, model.Tax, model.StartDate));

        private SalaryReviewDto MapEntityToDto(EmployeeSalaryReview employeeSalary)
        {
            var employee = employeeSalary.Employee;

            return new SalaryReviewDto
            {
                BasicSalary = employeeSalary.BasicSalary,
                EmployeeId = employeeSalary.EmployeeId,
                EndDate = employeeSalary.EndDate,
                HouseRent = employeeSalary.HouseRent,
                Id = employeeSalary.Id,
                Insurance = employeeSalary.Insurance,
                MealAllowance = employeeSalary.MealAllowance,
                ProvidenceFund = employeeSalary.ProvidenceFund,
                MedicalAllowance = employeeSalary.MedicalAllowance,
                StartDate = employeeSalary.StartDate,
                Tax = employeeSalary.Tax,
                TransportAllowance = employeeSalary.TransportAllowance,
                EmployeeInfo = new BaseEmployeeDto(employee.Id, employee.User.FirstName, employee.User.LastName, employee.Store.Name, employee.Designation.Name, employee.User.Email, employee.User.PhoneNumber, employee.User.IsActive, employee.User.PhotoUrl)
            };
        }

        public bool Exists(int businessId, int id)
        {
            return _repository.Exits(businessId, id);
        }
    }
}
