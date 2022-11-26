using smartshop.Entities;
using smartshop.Entities.SalesManagement;
using smartshop.Common.QueryParams;

namespace smartshop.Business.Service
{
    internal class SaleReturnService : ISaleReturnService
    {
        private readonly ISalesReturnRepository _repository;
        private readonly IStockRepository _stockRepository;

        public SaleReturnService(ISalesReturnRepository repository, IStockRepository stockRepository)
        {
            this._repository = repository;
            this._stockRepository = stockRepository;
        }
        public int Create(SalesReturnViewModel entity, string userId, string clientIp, int businessId)
        {
            string invoiceNumber = _repository.GeneratSaleInvoiceNumber(businessId);

            var purchaseReturn = new SaleReturn(entity.SaleId, entity.Date, invoiceNumber, userId, clientIp)
            {
                PreviousDue = entity.PreviousDue,
                SaleReturnProducts = entity.Products.Select(pr => new SaleReturnProduct()
                {
                    SaleProductId = pr.SaleProductId,
                    Qty = pr.Qty
                }).ToList()
            };

            _repository.Create(purchaseReturn);
            _stockRepository.UpdateStockForSaleReturn(purchaseReturn.SaleReturnProducts);
            return purchaseReturn.Id;
        }

        public void Delete(int id)
        {
            _stockRepository.ReverseStockForSaleReturn(id);
            _repository.Delete(id);
        }

        public bool Exists(int businessId, int id)
        {
            return _repository.Exists(businessId, id);
        }

        public PaginationResponse<SaleReturnDto> Get(int businessId, int currentPage, int pageSize, SalesQueryParams searchParams)
        {
            var response = _repository.Get(businessId, currentPage, pageSize, searchParams);
            return new PaginationResponse<SaleReturnDto>()
            {
                TotalRows = response.TotalRows,
                Rows = response.Rows.Select(pr => MapEntityToDto(pr))
            };
        }

        public IEnumerable<SaleReturnDto> Get(int businessId, SalesQueryParams searchParams)
            => _repository.Get(businessId, searchParams).Select(pr => MapEntityToDto(pr));

        public SaleReturnDetailsDto? Get(int businessId, int id)
        {
            var saleReturn = _repository.Get(businessId, id);
            if (saleReturn == null)
                return null;

            var saleReturnProducts = _repository.GetSaleReturnProducts(id);

            return new SaleReturnDetailsDto()
            {
                Id = saleReturn.Id,
                Date = saleReturn.Date,
                SaleInvoiceNumber = saleReturn.Sale.InvoiceNumber,
                ReturnInvoiceNumber = saleReturn.InvoiceNumber,
                StoreId = saleReturn.Sale.StoreId,
                SaleId = saleReturn.SaleId,
                StoreName = saleReturn.Sale.Store.Name,
                CustomerId = saleReturn.Sale.CustomerId,
                CustomerName = saleReturn.Sale.Customer.Head.Name,
                PurchaseReturnBy = saleReturn.CreatedBy,
                ReturnBill = saleReturnProducts.Sum(x => x.GetNetAmount()),
                PreviousDue = saleReturn.PreviousDue,
                CustomerIdentifier = saleReturn.Sale.Customer.Identifier,
                CustomerAddress = saleReturn.Sale.Customer.Address,
                CustomerEmail = saleReturn.Sale.Customer.Email,
                CustomerMobile = saleReturn.Sale.Customer.Mobile,
                Products = saleReturnProducts.Select(x => new SaleReturnProductDto()
                {
                    Id = x.Id,
                    saleProductId = x.SaleProductId,
                    Product = $"{x.SaleProduct.Stock.Product.Head.Name}-{x.SaleProduct.Stock.Product.Category.Name}-{x.SaleProduct.Stock.Product.Category.Group.Name}",
                    Qty = x.Qty,
                    UnitVariation = x.SaleProduct.UnitVariation.Name,
                    Price = x.SaleProduct.Price,
                    NetAmount = x.GetNetAmount()
                }).ToList(),
            };
        }

        public void Update(int id, SalesReturnViewModel entity)
        {
            var purchaseReturn = new SaleReturn(entity.SaleId, entity.Date, "", "", "")
            {
                PreviousDue = entity.PreviousDue,
                SaleReturnProducts = entity.Products.Select(pr => new SaleReturnProduct()
                {
                    SaleReturnId = id,
                    SaleProductId = pr.SaleProductId,
                    Qty = pr.Qty
                }).ToList()
            };

            _stockRepository.ReverseStockForSaleReturn(id);
            _repository.Update(id, purchaseReturn);
        }

        private SaleReturnDto MapEntityToDto(SaleReturn purchaseReturn)
        {
            return new SaleReturnDto
            {
                Id = purchaseReturn.Id,
                Date = purchaseReturn.Date,
                SaleInvoiceNumber = purchaseReturn.Sale.InvoiceNumber,
                ReturnInvoiceNumber = purchaseReturn.InvoiceNumber,
                StoreId = purchaseReturn.Sale.StoreId,
                StoreName = purchaseReturn.Sale.Store.Name,
                CustomerId = purchaseReturn.Sale.CustomerId,
                CustomerName = purchaseReturn.Sale.Customer.Head.Name,
                ReturnBill = purchaseReturn.SaleReturnProducts.Sum(x => x.GetNetAmount())

            };
        }
    }
}
