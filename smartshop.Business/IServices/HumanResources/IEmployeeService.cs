using smartshop.Business.ViewModels.HumanResources;

namespace smartshop.Business.IServices
{
    public interface IEmployeeService
    {
        int Create(EmployeeAddViewModel model);
        int Create(EmployeeViewModel model, string userId);
        string Update(string userId, EmployeePersonalInfoViewModel model);
        int Update(int id, EmployeeViewModel model);
        bool Exists(int businessId, int id);
        bool UserExistsByEmail(string email);
        bool UserExistsByPhone(string phoneNumber);
        bool ChangeStatus(int businessId, int id, bool status);
        PaginationResponse<BaseEmployeeDto> Get(int businessId, int currentPage, int pageSize, int? storeId = null);
        EmployeeDetailsDto? Get(int businessId, int id);
        PaginationResponse<EmployeeDto> EmployeeHistories(int businessId, int currentPage, int pageSize, int? storeId = null);

    }
}
