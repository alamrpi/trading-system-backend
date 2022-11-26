using smartshop.Common.Enums.Accounting;
using smartshop.Data.IRepositories.Businesses;
using smartshop.Entities.Accounting;
using smartshop.Entities.SalesManagement;

namespace smartshop.Business.Service
{
    internal class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IBusinessRepository _businessRepository;

        public CustomerService(ICustomerRepository repository, IBusinessRepository businessRepository)
        {
            _repository = repository;
            _businessRepository = businessRepository;
        }
        public int Create(int businessId, CustomerViewModel entity)
        {
            var head = new Head(businessId, entity.Name, entity.Descriptions, HeadTypes.Customer);

            var totalCustomers = _repository.TotalCount(businessId);
            var config = _businessRepository.GetConfigure(businessId);
            var customerId = $"{config.CustomerIdPrefix}-{totalCustomers + 1}";

            return _repository.Create(new Customer(customerId, entity.Mobile, entity.Email, entity.Address, entity.Type)
            {
                Head = head,
            });
        }

        public void Delete(int id)
            => _repository.Delete(id);

        public bool Exists(int businessId, int id)
            => _repository.Exists(businessId, id);

        public bool Exists(int businessId, string name, int? id = null)
            => _repository.Exists(businessId, name, id);

        public PaginationResponse<CustomerDto> Get(int businessId, int currentPage, int pageSize)
        {
            var response = _repository.Get(businessId, currentPage, pageSize);
            return new PaginationResponse<CustomerDto>
            {
                TotalRows = response.TotalRows,
                Rows = response.Rows.Select(b => MapEntityToDto(b)).ToList()
            };
        }

        public CustomerDto? Get(int businessId, int id)
        {
            var customer = _repository.Get(businessId, id);

            if (customer == null) return null;

            return MapEntityToDto(customer);
        }

        public IEnumerable<DropdownDto> Get(int businessId)
            => _repository.Get(businessId).Select(b => new DropdownDto($"{b.Head.Name} ({b.Mobile})", b.Id)).ToList();

        public void Update(int id, CustomerViewModel entity)
        {
            var head = new Head(0, entity.Name, entity.Descriptions, HeadTypes.Customer);
            _repository.Update(id, new Customer("", entity.Mobile, entity.Email, entity.Address, entity.Type)
            {
                Head = head,
            });
        }

        private CustomerDto MapEntityToDto(Customer b)
        {
            return new CustomerDto
            {
                Id = b.Id,
                Name = b.Head.Name,
                Email = b.Email,
                Mobile = b.Mobile,
                Address = b.Address,
                Descriptions = b.Head.Descriptions,
                Identifier = b.Identifier,
                Type = b.Type
            };
        }
    }
}
