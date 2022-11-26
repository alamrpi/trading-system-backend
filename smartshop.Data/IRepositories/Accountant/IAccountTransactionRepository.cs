using smartshop.Common.Enums.Accounting;
using smartshop.Common.QueryParams.Accountant;
using smartshop.Entities.Accounting;

namespace smartshop.Data.IRepositories
{
    public interface IAccountTransactionRepository
    {
        IEnumerable<AccountTransaction> GetBalaceSheet(int businessId, StatementQueryParams queryParams);
        IEnumerable<AccountTransaction> GetTransactionByAccount(int accountId, int businessId, StatementQueryParams queryParams);
        IEnumerable<AccountTransaction> GetTransactionByComponent(EquationComponent component, int businessId, StatementQueryParams queryParams);
    }
}
