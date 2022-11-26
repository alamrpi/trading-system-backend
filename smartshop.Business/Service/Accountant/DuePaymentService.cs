using smartshop.Business.IServices.Businesses;
using smartshop.Common.QueryParams;
using smartshop.Entities.Accounting;

namespace smartshop.Business.Service
{
    internal class DuePaymentService : IDuePaymentService
    {
        private readonly IDuePaymentRepository _repository;
        private readonly IStoreService _storeService;
        private readonly ISupplierService _supplierService;
        private readonly IBankAccountService _bankAccountService;

        public DuePaymentService(IDuePaymentRepository repository, IStoreService storeService, ISupplierService supplierService, IBankAccountService bankAccountService)
        {
            _repository = repository;
            _storeService = storeService;
            _supplierService = supplierService;
            this._bankAccountService = bankAccountService;
        }
        public int Create(int businessId, DuePaymentViewModel entity, string ip, string userId)
        {
            string slipNumber = _repository.GenerateSlip(businessId);
            return _repository.Create(new DuePayment(entity.PurchaseId, entity.Date, entity.Amount, entity.Discount, entity.DiscountType, slipNumber, ip, userId)
            {
                PreviousDue = entity.PreviousDue,
                BankAccountId = entity.BankAccountId
            });
        }

        public void Delete(int id)
            => _repository.Delete(id);

        public bool Exists(int businessId, int id)
            => _repository.Exists(businessId, id);

        public PaginationResponse<DuePaymentDto> Get(int businessId, DuePaymentQueryParams queryParams)
        {
            var response = _repository.Get(businessId, queryParams);
            return new PaginationResponse<DuePaymentDto>
            {
                TotalRows = response.TotalRows,
                Rows = response.Rows.Select(b => MapEntityToDto(b)).ToList()
            };
        }

        public DuePaymentDto? Get(int businessId, int id)
        {
            var bankAccount = _repository.Get(businessId, id);

            if (bankAccount == null) return null;

            return MapEntityToDto(bankAccount);
        }

        public DuePaymentDdlDataDto GetDdlData(int businessId)
        {
            return new DuePaymentDdlDataDto
            {
                Store = _storeService.Gets(businessId),
                Suppliers = _supplierService.Get(businessId),
                Banks = _bankAccountService.Get(businessId),
            };
        }

        public void Update(int id, DuePaymentViewModel entity)
        {
            _repository.Update(id, new DuePayment()
            {
                PurchaseId = entity.PurchaseId,
                Amount = entity.Amount,
                BankAccountId = entity.BankAccountId,
                Date = entity.Date,
                Discount = entity.Discount,
                DiscountType = entity.DiscountType,
                PreviousDue = entity.PreviousDue,
            });
        }

        private DuePaymentDto MapEntityToDto(DuePayment b)
        {
            return new DuePaymentDto
            {
                Id = b.Id,
                DiscountType = b.DiscountType,
                 Amount = b.Amount,
                 BankAccountName = b.BankAccount.Head.Name,
                 BankAccountId = b.BankAccount.Id,
                 Date = b.Date,
                 Discount = b.Discount,
                 DiscountAmount = b.GetDiscountAmount(),
                 PurchaseInvoiceNumber = b.Purchase.InvoiceNumber,
                 PurchaseId = b.PurchaseId,
                 StoreId = b.Purchase.StoreId,
                 SlipNumber = b.PaymentSlipNumber,
                 StoreName = b.Purchase.Store.Name,
                 SupplierName = b.Purchase.Supplier.Head.Name,
                 SupplierId = b.Purchase.SupplierId,
                 PreviousDue = b.PreviousDue
            };
        }
    }
}
