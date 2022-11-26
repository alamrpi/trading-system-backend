using smartshop.Business.IServices.Businesses;
using smartshop.Common.QueryParams;
using smartshop.Entities.Accounting;

namespace smartshop.Business.Service
{
    internal class DueCollectionService : IDueCollectionService
    {
        private readonly IDueCollectionRepository _repository;
        private readonly IStoreService _storeService;
        private readonly ICustomerService _customerService;
        private readonly IBankAccountService _bankAccountService;

        public DueCollectionService(IDueCollectionRepository repository, IStoreService storeService, ICustomerService customerService, IBankAccountService bankAccountService)
        {
            _repository = repository;
            _storeService = storeService;
            _customerService = customerService;
            _bankAccountService = bankAccountService;
        }
        public int Create(DueCollectionViewModel entity, string ip, string userId, int businessId)
        {
            string slipNumber = _repository.GenerateSerialNo(businessId);
            return _repository.Create(new DueCollection(entity.SaleId, entity.BankAccountId, entity.Date, entity.Amount, entity.Discount, entity.DiscountType, slipNumber, userId, ip)
            {
                PreviouseDue = entity.PreviousDue
            });
        }

        public void Delete(int id)
            => _repository.Delete(id);

        public bool Exists(int businessId, int id)
            => _repository.Exists(businessId, id);

        public PaginationResponse<DueCollectionDto> Get(int businessId, DueCollectionQueryParams queryParams)
        {
            var response = _repository.Get(businessId, queryParams);
            return new PaginationResponse<DueCollectionDto>
            {
                TotalRows = response.TotalRows,
                Rows = response.Rows.Select(b => MapEntityToDto(b)).ToList()
            };
        }

        public DueCollectionDto? Get(int businessId, int id)
        {
            var dueCollections = _repository.Get(businessId, id);

            if (dueCollections == null) return null;

            return MapEntityToDto(dueCollections);
        }

        public DueCollectionDdlDataDto GetDdlData(int businessId)
        {
            return new DueCollectionDdlDataDto
            {
                Store = _storeService.Gets(businessId),
                Customers = _customerService.Get(businessId),
                Banks = _bankAccountService.Get(businessId),
            };
        }

        public void Update(int id, DueCollectionViewModel entity)
        {
            _repository.Update(id, new DueCollection()
            {
                SaleId = entity.SaleId,
                Amount = entity.Amount,
                BankAccountId = entity.BankAccountId,
                Date = entity.Date,
                Discount = entity.Discount,
                DiscountType = entity.DiscountType,
                PreviouseDue = entity.PreviousDue,
            });
        }

        private DueCollectionDto MapEntityToDto(DueCollection dueCollections)
        {
            return new DueCollectionDto
            {
                Id = dueCollections.Id,
                DiscountType = dueCollections.DiscountType,
                Amount = dueCollections.Amount,
                BankAccountName = dueCollections.BankAccount.Head.Name,
                BankAccountId = dueCollections.BankAccountId,
                CustomerId = dueCollections.Sale.CustomerId,
                SaleId = dueCollections.SaleId,
                StoreId = dueCollections.Sale.StoreId,
                Date = dueCollections.Date,
                Discount = dueCollections.Discount,
                DiscountAmount = dueCollections.GetDiscountAmount(),
                SaleInvoiceNumber = dueCollections.Sale.InvoiceNumber,
                SlipNumber = dueCollections.SlipNumber,
                StoreName = dueCollections.Sale.Store.Name,
                CustomerName = dueCollections.Sale.Customer.Head.Name,
                PreviousDue = dueCollections.PreviouseDue
            };
        }
    }
}
