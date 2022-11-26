using smartshop.Business.ViewModels.HumanResources;

namespace smartshop.Business.IServices.HumanResources
{
    public interface ISalaryReviewService
    {
        PaginationResponse<SalaryReviewDto> Get(int businessId, int currentPage, int pageSize, int? storeId);
        SalaryReviewDto? Get(int businessId, int id);
        SalaryReviewDto? GetByEmployeeId(int businessId, int employeeId);
        int Create(int employeeId, SalaryReviewViewModel model);
        int Update(int businessId, int id, SalaryReviewViewModel model);
        bool Exists(int businessId, int id);
    }
}
