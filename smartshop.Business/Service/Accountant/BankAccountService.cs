using smartshop.Common.Enums.Accounting;
using smartshop.Entities.Accounting;

namespace smartshop.Business.Service.Accountant
{
    internal class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepsitory _repository;
     
        public BankAccountService(IBankAccountRepsitory repository)
        {
            _repository = repository;
        }
        public int Create(int businessId, BankAccountViewModel entity)
        {
            var head = new Head(businessId, entity.Name, entity.Descriptions, HeadTypes.BankAccount);

            return _repository.Create(new BankAccount(head, entity.BranchName, entity.AccountNumber, entity.Balance));
        }

        public void Delete(int id)
            => _repository.Delete(id);

        public bool Exists(int businessId, int id)
            => _repository.Exists(businessId, id);

        public bool Exists(int businessId, string name, int? id = null)
            => _repository.Exists(businessId, name, id);

        public PaginationResponse<BankAccountDto> Get(int businessId, int currentPage, int pageSize)
        {
            var response = _repository.Get(businessId, currentPage, pageSize);
            return new PaginationResponse<BankAccountDto>
            {
                TotalRows = response.TotalRows,
                Rows = response.Rows.Select(b => MapEntityToDto(b)).ToList()
            };
        }

        public BankAccountDto? Get(int businessId, int id)
        {
            var bankAccount = _repository.Get(businessId, id);

            if (bankAccount == null) return null;

            return MapEntityToDto(bankAccount);
        }

        public IEnumerable<DropdownDto> Get(int businessId)
            => _repository.Get(businessId).Select(b => new DropdownDto(b.Head.Name, b.Id)).ToList();

        public void Update(int id, BankAccountViewModel entity)
        {
            var head = new Head(0, entity.Name, entity.Descriptions, HeadTypes.Supplier);
            _repository.Update(id, new BankAccount(head, entity.BranchName, entity.AccountNumber, entity.Balance));
        }

        private BankAccountDto MapEntityToDto(BankAccount b)
        {
            return new BankAccountDto
            {
                Id = b.Id,
                BankAccountName = b.Head.Name,
                AccountNumber = b.AccountNumber,
                Balance = b.Balance,
                BranchName = b.BranchName,
                Descriptions = b.Head.Descriptions
            };
        }
    }
}
