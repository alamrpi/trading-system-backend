using smartshop.Business.Dtos.Accountant;
using smartshop.Business.ViewModels.Accountant;
using smartshop.Common.QueryParams;
using smartshop.Common.QueryParams.Accountant;

namespace smartshop.Business.IServices
{
    public interface IIncomeExpenseService
    {
        PaginationResponse<IncomeExpenseDto> Get(int businessId, IncomeExpenseQueryParams queryParams);
        IncomeExpenseDto? Get(int businessId, int id);
        bool Exists(int businessId, int id);
        int Create(IncomeExpenseViewModel entity);
        void Update(int id, IncomeExpenseViewModel entity);
        void Delete(int id);
    }
}
