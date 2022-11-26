using smartshop.Business.Dtos.Accountant;
using smartshop.Common.Enums.Accounting;
using smartshop.Common.QueryParams.Accountant;

namespace smartshop.Business.IServices
{
    public interface IAccountTransactionService
    {
        IEnumerable<AccountTransactionDto> GetBalanceSheet(int businessId, StatementQueryParams queryParams);
        IEnumerable<AccountTransactionDto> GetTransactionByAccount(int accountId, int businessId, StatementQueryParams queryParams);
        IEnumerable<AccountTransactionDto> GetTransactionByComponent(EquationComponent component, int businessId, StatementQueryParams queryParams);
    }
}
