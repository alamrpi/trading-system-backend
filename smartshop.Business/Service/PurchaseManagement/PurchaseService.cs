using smartshop.Business.Dtos.PurchaseManagement;
using smartshop.Business.IServices.Businesses;
using smartshop.Common.QueryParams;
using smartshop.Common.QueryParams.Purchase;
using smartshop.Entities;
using smartshop.Entities.PurchaseManagement;

namespace smartshop.Business.Service
{
    internal class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _repository;
        private readonly IStockRepository _stockRepository;
        private readonly ISupplierService _supplierService;
        private readonly IStoreService _storeService;
        private readonly IProductService _productService;
        private readonly IBankAccountService _bankAccountService;

        public PurchaseService(IPurchaseRepository purchaseRepository, IStockRepository stockRepository, 
            ISupplierService supplierService, IStoreService storeService, IProductService productService, IBankAccountService bankAccountService)
        {
            _repository = purchaseRepository;
            _stockRepository = stockRepository;
            _supplierService = supplierService;
            _storeService = storeService;
            _productService = productService;
            _bankAccountService = bankAccountService;
        }
        public bool AnyDuePaymentOrReturn(int businessId, int id)
        {
            return _repository.AnyDuePaymentOrReturn(businessId, id);
        }

        public int Create(PurchaseViewModel model, string userId, string clientIp, int businessId)
        {
            var purchase = new Purchase(model.SupplierId, model.StoreId, model.BankAccountId, model.InvoiceNumber, model.Date, model.Discount, model.DiscountType, model.Overhead, model.Paid, clientIp, userId)
            {
                PurchaseProducts = model.PurchaseProducts.Select(pp => new PurchaseProduct(pp.ProductId, pp.UnitVariationId, pp.Qty, pp.BonusQty, pp.PurchasePrice, pp.Vat, pp.TradePrice)).ToList()
            };

            var id = _repository.Create(purchase, businessId);

            _stockRepository.UpdateStockWhenPurchase(purchase.PurchaseProducts, model.StoreId);

            return id;
        }

        public void Delete(int id)
        {
            _stockRepository.ReverseStockForPurchase(id);
            _repository.Delete(id);
        }

        public bool Exists(int businessId, int id)
        {
            return _repository.Exists(businessId, id);
        }

        public PaginationResponse<PurchaseDto> Get(int businessId, int currentPage, int pageSize, PurchaseQueryParams searchParams)
        {
            var purchaseResponse = _repository.Get(businessId, currentPage, pageSize, searchParams);

            return new PaginationResponse<PurchaseDto>(purchaseResponse.TotalRows, purchaseResponse.Rows.Select(p => MapEntityToDto(p)));
          
        }

        public IEnumerable<DropdownDto> Get(int businessId, int? storeId, int? supplierId)
        {
            return _repository.Get(businessId, storeId, supplierId)
                .Select(p => new DropdownDto()
                {
                    Text = $"{p.InvoiceNumber} - {p.Supplier.Head.Name} - {Math.Round(p.GetCurrentDue(), 2)}",
                    Value = p.Id
                });
        }

        public IEnumerable<PurchaseDto> Get(int businessId, PurchaseQueryParams searchParams)
        {
            return _repository.Get(businessId, searchParams).Select(p => MapEntityToDto(p));
        }

        public PurchaseDetailsDto? Get(int businessId, int id)
        {
            var purchase = _repository.Get(businessId, id);
            if (purchase == null)
                return null;

            return new PurchaseDetailsDto
            {
                Id = purchase.Id,
                BankAccountId = purchase.BankAccountId,
                BankAccountName = purchase.BankAccount.Head.Name,
                SupplierId = purchase.SupplierId,
                SupplierName = purchase.Supplier.Head.Name,
                SupId = purchase.Supplier.SupId,
                SupplierPhone = purchase.Supplier.Mobile,
                SupplierEmail = purchase.Supplier.Email,
                SupplierAddress = purchase.Supplier.Address,
                StoreName = purchase.Store.Name,
                StoreId = purchase.StoreId,
                StoreAddress = purchase.Store.Address,
                StoreContact = purchase.Store.ContactNo,
                StoreEmail = purchase.Store.Email,
                Date = purchase.Date,
                CreatedAt = purchase.CreatedAt,
                CreatedBy = purchase.CreatedBy,
                CreatedIp = purchase.CreatedIp,
                Discount = purchase.Discount,
                DiscountType = purchase.DiscountType,
                InvoiceNumber = purchase.InvoiceNumber,
                Overhead = purchase.Overhead,
                Paid = purchase.Paid,
                PurchaseProducts = purchase.PurchaseProducts.Select(p => new PurchaseProductDto()
                {
                    Id = p.Id,
                    BonusQty = p.BonusQty,
                    ProductId = p.ProductId,
                    ProductName = p.Product.Head.Name,
                    PurchasePrice = p.PurchasePrice,
                    Qty = p.Qty,
                    TradePrice = p.TradePrice,
                    UnitVariation = p.UnitVariation.Name,
                    UnitVariationId = p.UnitVariationId,
                    Vat = p.Vat,
                    TotalVat = p.GetTotalVat(),
                    TotalPurchasePrice = p.GetTotalPurchsePrice()
                }).ToList(),

                DiscountAmount = purchase.GetDiscountAmount(),
                Due = purchase.GetDue(),
                GrossTradePrice = purchase.GetGrossTradePrice(),
                GrossVat = purchase.GetGrossVat(),
                NetPayableAmount = purchase.GetPayableAmount()
            };
        }

        public PurchaseDdlDto GetPurchaseDdl(int businessId)
        {
            return new PurchaseDdlDto
            {
                Suppliers = _supplierService.Get(businessId),
                Stores =  _storeService.Gets(businessId),
                Products = _productService.Get(businessId, new ProductQueryParams()),
                BankAccounts = _bankAccountService.Get(businessId),
            };
        }

        public IEnumerable<PurchaseProductDto> GetPurchaseProducts(int businessId, int id)
        {
            return _repository.GetPurchaseProducts(businessId, id).Select(p => new PurchaseProductDto
            {
                Id = p.Id,
                BonusQty = p.BonusQty,
                ProductId = p.ProductId,
                ProductName = p.Product.Head.Name,
                PurchasePrice = p.PurchasePrice,
                Qty = p.Qty,
                TradePrice = p.TradePrice,
                UnitVariation = p.UnitVariation.Name,
                UnitVariationId = p.UnitVariationId,
                Vat = p.Vat,
                TotalVat = p.GetTotalVat(),
                TotalPurchasePrice = p.GetTotalPurchsePrice()
            });
        }

        public void Update(int id, PurchaseViewModel model)
        {
            _stockRepository.ReverseStockForPurchase(id);

            _repository.Update(id, new Purchase(model.SupplierId, model.StoreId, model.BankAccountId, model.InvoiceNumber, model.Date, model.Discount, model.DiscountType, model.Overhead, model.Paid, "", "")
            {
                PurchaseProducts = model.PurchaseProducts.Select(pp => new PurchaseProduct(pp.ProductId, pp.UnitVariationId, pp.Qty, pp.BonusQty, pp.PurchasePrice, pp.Vat, pp.TradePrice)
                {
                    PurchaseId = id,
                }).ToList()
            });
        }

        private PurchaseDto MapEntityToDto(Purchase p)
        {
            return new PurchaseDto()
            {
                BankAccountId = p.BankAccountId,
                InvoiceNumber = p.InvoiceNumber,
                Date = p.Date,
                BankAccountName = p.BankAccount.Head.Name,
                Paid = p.Paid,
                StoreId = p.StoreId,
                StoreName = p.Store.Name,
                SupplierId = p.SupplierId,
                SupplierName = p.Supplier.Head.Name,
                Id = p.Id
            };
        }
    }
}
