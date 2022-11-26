using smartshop.Common.QueryParams.Accountant;
using smartshop.Entities.Accounting;

namespace smartshop.Data.IRepositories
{
    public interface IIncomeExpenseRepository
    {
        PaginationResponse<IncomeExpense> Gets(int businessId, IncomeExpenseQueryParams paginateQueryParams);
        IncomeExpense? Get(int businessId, int id);
        bool Exists(int businessId, int id);
        int Create(IncomeExpense entity);
        void Update(int id, IncomeExpense entity);
        void Delete(int id);
    }
}
