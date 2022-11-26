using smartshop.Common.Dto;
using smartshop.Entities.HumanResource;

namespace smartshop.Data.IRepositories
{
    public interface IEmployeeRepository
    {
        int Create(Employee employee);
        string Create(EmployeePersonalInfo personalInfo);
        int Update(Employee employee);
        string Update(string userId, EmployeePersonalInfo employeePersonalInfo);
        Employee? Get(int businessId, int id);
        bool Exists(int businessId, int id);
        IEnumerable<Employee> Get(int businessId, int? storeId = null);
        PaginationResponse<Employee> Get(int businessId, int currentPage, int pageSize, int? storeId = null, bool isCurrent = true);
        bool ChangeStatus(int businessId, int id, bool status);
    }
}
