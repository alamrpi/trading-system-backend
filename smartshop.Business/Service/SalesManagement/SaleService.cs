using smartshop.Business.Dtos.SalesManagement;
using smartshop.Common.QueryParams;
using smartshop.Data.IRepositories.Businesses;
using smartshop.Entities.SalesManagement;

namespace smartshop.Business.Service
{
    internal class SaleService : ISaleService
    {
        private readonly ISaleRepository _repository;
        private readonly IStockRepository _stockRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IBankAccountRepsitory _bankAccountRepsitory;
        private readonly IStoreRepository _storeRepository;

        public SaleService(ISaleRepository purchaseRepository, IStockRepository stockRepository,
            ICustomerRepository customerRepository, IBankAccountRepsitory bankAccountRepsitory, IStoreRepository storeRepository)
        {
            _repository = purchaseRepository;
            _stockRepository = stockRepository;
            this._customerRepository = customerRepository;
            this._bankAccountRepsitory = bankAccountRepsitory;
            this._storeRepository = storeRepository;
        }
        public bool AnyDueCollectionOrReturn(int businessId, int id)
        {
            return _repository.AnyDueCollectionOrReturn(businessId, id);
        }

        public int Create(SaleViewModel model, string userId, string clientIp, int businessId)
        {
            string invoiceNumber = _repository.GeneratSaleInvoiceNumber(businessId);
            var sales = new Sale(model.CustomerId, model.StoreId, model.Date, invoiceNumber, model.Vat, model.Discount, model.DiscountType, model.Overhead, model.Paid, model.BankAccountId, clientIp, userId)
            {
                SaleProducts = model.Products.Select(sl => new SaleProduct()
                {
                    StockId = sl.StockId,
                    UnitVariationId = sl.UnitVariationId,
                    Qnty = sl.Qnty,
                    Price = sl.Price,
                }).ToList()
            };

            var id = _repository.Create(sales);

            _stockRepository.UpdateStockWhenSale(sales.SaleProducts);

            return id;
        }

        public void Delete(int id)
        {
            _stockRepository.ReverseStockForSale(id);
            _repository.Delete(id);
        }

        public bool Exists(int businessId, int id)
        {
            return _repository.Exists(businessId, id);
        }

        public PaginationResponse<SaleDto> Get(int businessId, int currentPage, int pageSize, SalesQueryParams searchParams)
        {
            var purchaseResponse = _repository.Get(businessId, currentPage, pageSize, searchParams);

            return new PaginationResponse<SaleDto>(purchaseResponse.TotalRows, purchaseResponse.Rows.Select(p => MapEntityToDto(p)));
        }
        
        public IEnumerable<DropdownDto> Get(int businessId, int? storeId, int? customerId)
        {
            return _repository.Get(businessId, storeId, customerId).Select(p => new DropdownDto()
            {
                Text = $"{p.InvoiceNumber}-{p.Customer.Head.Name}-{Math.Round(p.GetCurrentDue())}",
                Value = p.Id
            });
        }

        public IEnumerable<SaleDto> Get(int businessId, SalesQueryParams searchParams)
        {
            return _repository.Get(businessId, searchParams).Select(p => MapEntityToDto(p));
        }

        public SaleDetailsDto? Get(int businessId, int id)
        {
            var sale = _repository.Get(businessId, id);
            if (sale == null)
                return null;

            return new SaleDetailsDto
            {
                Id = sale.Id,
                BankAccountId = sale.BankAccountId,
                BankAccountName = sale.BankAccount.BranchName,
                CustomerName = sale.Customer.Head.Name,
                CustomerId = sale.CustomerId,
                CustomerAddress = sale.Customer.Address,
                CustomerMobile = sale.Customer.Mobile,
                StoreName = sale.Store.Name,
                StoreId = sale.StoreId,
                StoreAddress = sale.Store.Address,
                StoreContact = sale.Store.ContactNo,
                StoreEmail = sale.Store.Email,
                Date = sale.Date,
                Discount = sale.Discount,
                DiscountType = sale.DiscountType,
                DiscountAmount = sale.GetDiscountAmount(),
                InvoiceNumber = sale.InvoiceNumber,
                GrandTotal = sale.GrandTotal(),
                Overhead = sale.Overhead,
                Paid = sale.Paid,
                Due = sale.GetDueAmount(),
                PayableAmount = sale.GetPayableAmount(),
                Vat = sale.Vat,
                VatAmount = sale.GetVatAmount(),
                EntryAt = sale.CreatedAt,
                EntryBy = sale.CreatedBy,
                Products = sale.SaleProducts.Select(p => new SaleProductDto()
                {
                   Id = p.Id,
                   StockId = p.StockId,
                   Price = p.Price,
                   ProductDetails = $"{p.Stock.Product.Head.Name} - {p.Stock.Product.Category.Name} - {p.Stock.Product.Category.Group.Name}",
                   Qnty = p.Qnty,
                   UnitVariatonId = p.UnitVariationId,
                   UnitVariatonName = p.UnitVariation.Name,
                   TotalAmount = p.GetTotalAmount()
                }).ToList(),
            };
        }

        public SalesDdlDto GetDdlData(int businessId)
        {
            return new SalesDdlDto
            {
                Banks = _bankAccountRepsitory.Get(businessId).Select(b => new DropdownDto(b.Head.Name, b.Id)),
                Stores = _storeRepository.Gets(businessId).Select(s => new DropdownDto(s.Name, s.Id)),
                Customers = _customerRepository.Get(businessId).Select(c => new DropdownDto($"{c.Head.Name}-{c.Mobile}", c.Id))
            };
        }

        public IEnumerable<SaleProductDto> GetSaleProducts(int businessId, int id)
        {
            return _repository.GetSaleProduct(businessId, id).Select(p => new SaleProductDto()
            {
                Id = p.Id,
                StockId = p.StockId,
                Price = p.Price,
                ProductDetails = $"{p.Stock.Product.Head.Name} - {p.Stock.Product.Category.Name} - {p.Stock.Product.Category.Group.Name}",
                Qnty = p.Qnty,
                UnitVariatonId = p.UnitVariationId,
                UnitVariatonName = p.UnitVariation.Name,
                TotalAmount = p.GetTotalAmount()
            }).ToList();
        }

        public void Update(int id, SaleViewModel model)
        {
            _stockRepository.ReverseStockForSale(id);

            _repository.Update(id, new Sale(model.CustomerId, model.StoreId, model.Date, "", model.Vat, model.Discount, model.DiscountType, model.Overhead, model.Paid, model.BankAccountId, "", "")
            {
                SaleProducts = model.Products.Select(sl => new SaleProduct()
                {
                    SaleId = id,
                    StockId = sl.StockId,
                    UnitVariationId = sl.UnitVariationId,
                    Qnty = sl.Qnty,
                    Price = sl.Price,
                }).ToList()
            });
        }

        private SaleDto MapEntityToDto(Sale sale)
        {
            return new SaleDto()
            {
                Id = sale.Id,
                InvoiceNumber = sale.InvoiceNumber,
                Date = sale.Date,
                Paid = sale.Paid,
                StoreId = sale.StoreId,
                StoreName = sale.Store.Name,
                CustomerId = sale.CustomerId,
                CustomerName = sale.Customer.Head.Name,
                BankAccountId = sale.BankAccountId,
                BankAccountName = sale.BankAccount.Head.Name,
                PayableAmount = sale.GetPayableAmount(),
                Due = sale.GetDueAmount(),
            };
        }
    }
}
