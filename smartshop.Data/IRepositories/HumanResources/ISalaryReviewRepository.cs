using smartshop.Common.Dto;
using smartshop.Entities.HumanResource;

namespace smartshop.Data.IRepositories
{
    public interface ISalaryReviewRepository
    {
        PaginationResponse<EmployeeSalaryReview> Get(int businessId, int currentPage, int pageSize, int? storeId);
        EmployeeSalaryReview? Get(int businessId, int id);
        EmployeeSalaryReview? GetByEmployeeId(int businessId, int employeeId);
        int Create(EmployeeSalaryReview employeeSalaryReview);
        int Update(int businessId, int id, EmployeeSalaryReview employeeSalaryReview);
        bool Exits(int businessId, int id);
    }
}
