using smartshop.Business.Dtos.Accountant;
using smartshop.Common.Enums.Accounting;
using smartshop.Common.QueryParams.Accountant;
using smartshop.Entities.Accounting;

namespace smartshop.Business.Service.Accountant
{
    internal class AccountTransactionService : IAccountTransactionService
    {
        private readonly IAccountTransactionRepository _accountTransactionRepository;

        public AccountTransactionService(IAccountTransactionRepository accountTransactionRepository)
        {
            this._accountTransactionRepository = accountTransactionRepository;
        }

        public IEnumerable<AccountTransactionDto> GetBalanceSheet(int businessId, StatementQueryParams queryParams) 
            => _accountTransactionRepository.GetBalaceSheet(businessId, queryParams).Select(x => MapDto(x));
        public IEnumerable<AccountTransactionDto> GetTransactionByAccount(int accountId, int businessId, StatementQueryParams queryParams)
            => _accountTransactionRepository.GetTransactionByAccount(accountId, businessId, queryParams).Select(x => MapDto(x));

        public IEnumerable<AccountTransactionDto> GetTransactionByComponent(EquationComponent component, int businessId, StatementQueryParams queryParams)
            => _accountTransactionRepository.GetTransactionByComponent(component, businessId, queryParams).Select(x => MapDto(x));

        private static AccountTransactionDto MapDto(AccountTransaction x)
        {
            return new AccountTransactionDto
            {
                AccountName = x.Account.Name,
                Amount = x.HeadTransaction.Amount,
                Descriptions = x.HeadTransaction.Descriptions,
                Operator = x.Operator,
                Component = x.Account.EquationComponent,
                HeadName = x.HeadTransaction.Head.Name,
               Date = x.HeadTransaction.Date.ToString("dd/MM/yyy")
            };
        }
    }
}
