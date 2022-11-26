using smartshop.Business.Dtos.Accountant;
using smartshop.Business.ViewModels.Accountant;
using smartshop.Common.QueryParams;
using smartshop.Common.QueryParams.Accountant;
using smartshop.Entities.Accounting;

namespace smartshop.Business.Service.Accountant
{
    internal class IncomeExpenseService : IIncomeExpenseService
    {
        private readonly IIncomeExpenseRepository _incomeExpenseRepository;

        public IncomeExpenseService(IIncomeExpenseRepository incomeExpenseRepository)
        {
            _incomeExpenseRepository = incomeExpenseRepository;
        }
        public int Create(IncomeExpenseViewModel entity)
        {
            return _incomeExpenseRepository.Create(new IncomeExpense
            {
                HeadId = entity.HeadId,
                BankAccountId = entity.BankAccountId,
                Amount = entity.Amount,
                Date = entity.Date,
                Description = entity.Description,
                Income = entity.Income
            });
        }

        public void Delete(int id)
        {
            _incomeExpenseRepository.Delete(id);
        }

        public bool Exists(int businessId, int id)
        {
            return _incomeExpenseRepository.Exists(businessId, id);
        }

        public PaginationResponse<IncomeExpenseDto> Get(int businessId, IncomeExpenseQueryParams queryParams)
        {
            var response = _incomeExpenseRepository.Gets(businessId, queryParams);
            return new PaginationResponse<IncomeExpenseDto>()
            {
                Rows = response.Rows.Select(i => MappingDto(i)),
                TotalRows = response.TotalRows,
            };
        }


        public IncomeExpenseDto? Get(int businessId, int id)
        {
            var incomeExpense = _incomeExpenseRepository.Get(businessId, id);

            if (incomeExpense == null)
                return null;

            return MappingDto(incomeExpense);
        }

        public void Update(int id, IncomeExpenseViewModel entity)
        {
            _incomeExpenseRepository.Update(id, new IncomeExpense()
            {
                HeadId = entity.HeadId,
                BankAccountId = entity.BankAccountId,
                Amount = entity.Amount,
                Date = entity.Date,
                Description = entity.Description,
                Income = entity.Income
            });
        }
        private static IncomeExpenseDto MappingDto(IncomeExpense i)
        {
            return new IncomeExpenseDto
            {
                Id = i.Id,
                Amount = i.Amount,
                BankAccountId = i.BankAccountId,
                BankAccountName = i.BankAccount.Head.Name,
                Date = i.Date,
                Description = i.Description,
                HeadId = i.HeadId,
                HeadName = i.Head.Name,
                Income = i.Income
            };
        }
    }
}
