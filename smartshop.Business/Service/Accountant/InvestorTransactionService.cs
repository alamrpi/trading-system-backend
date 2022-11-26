using smartshop.Business.Dtos.Accountant;
using smartshop.Common.QueryParams;
using smartshop.Entities.Accounting;

namespace smartshop.Business.Service
{
    internal class InvestorTransactionService : IInvestorTransactionService
    {
        private readonly IInvestorTransactionRepository _repository;
        private readonly IInvestorService _investorService;
        private readonly IBankAccountService _bankAccountService;

        public InvestorTransactionService(IInvestorTransactionRepository investorTransactionRepository, IInvestorService investorService, IBankAccountService bankAccountService)
        {
            this._repository = investorTransactionRepository;
            this._investorService = investorService;
            this._bankAccountService = bankAccountService;
        }
        public int Create(InvestorTransactionViewModel entity, string userId)
        {
            return _repository.Create(new InvestorTransaction
            {
                InvestorId = entity.InvestorId,
                BankAccountId = entity.BankAccountId,
                Amount = entity.Amount,
                Date = entity.Date,
                TransactionType = entity.TransactionType,
                CreatedBy = userId,
                Descriptions = entity.Descriptions
            });
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        public bool Exists(int businessId, int id) 
            => _repository.Exists(businessId, id);

        public PaginationResponse<InvestorTransactionDto> Get(int businessId, InvestorTransactionQueryParams queryParams)
        {
            var data = _repository.Get(businessId, queryParams);

            return new PaginationResponse<InvestorTransactionDto>()
            {
                Rows = data.Rows.Select(it => MapEntityToDto(it)),
                TotalRows = data.TotalRows
            };
        }

        public InvestorTransactionDto? Get(int businessId, int id) 
            => MapEntityToDto(_repository.Get(businessId, id));

        public InvestorTransactionDdlDataDto GetDdlData(int businessId)
        {
            return new InvestorTransactionDdlDataDto
            {
                Banks = _bankAccountService.Get(businessId),
                Investors = _investorService.Get(businessId)
            };
        }

        public void Update(int id, InvestorTransactionViewModel entity)
        {
            _repository.Update(id, new InvestorTransaction
            {
                InvestorId = entity.InvestorId,
                BankAccountId = entity.BankAccountId,
                Amount = entity.Amount,
                Date = entity.Date,
                TransactionType = entity.TransactionType,
                Descriptions = entity.Descriptions
            });
        }
        private static InvestorTransactionDto MapEntityToDto(InvestorTransaction it)
        {
            return new InvestorTransactionDto
            {
                InvestorId = it.InvestorId,
                Amount = it.Amount,
                BankAccountId = it.BankAccountId,
                Date = it.Date,
                BankAccountName = it.BankAccount.Head.Name,
                InvestorName = it.Investor.Head.Name,
                Descriptions = it.Descriptions,
                CreatedBy = it.CreatedBy,
                Id = it.Id,
                TransactionType = it.TransactionType
            };
        }
    }
}
