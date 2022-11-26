using smartshop.Common.QueryParams.Purchase;
using smartshop.Entities;
using smartshop.Entities.PurchaseManagement;

namespace smartshop.Business.Service
{
    internal class PurchaseReturnService : IPurchaseReturnService
    {
        private readonly IPurchaseReturnRepository _repository;
        private readonly IStockRepository _stockRepository;

        public PurchaseReturnService(IPurchaseReturnRepository repository, IStockRepository stockRepository)
        {
            this._repository = repository;
            this._stockRepository = stockRepository;
        }
        public int Create(PurchaseReturnViewModel entity, string userId, string clientIp)
        {
            var purchaseReturn = new PurchaseReturn(entity.PurchaseId, "", entity.Date, userId, clientIp)
            {
                PurchaseReturnProducts = entity.Products.Select(pr => new PurchaseReturnProduct()
                {
                    PurchaseProductId = pr.PurchaseProductId,
                    Qty = pr.Qty
                }).ToList()
            };

            _repository.Create(purchaseReturn);
            _stockRepository.UpdateStockForPurchaseReturn(purchaseReturn.PurchaseReturnProducts);
            return purchaseReturn.Id;
        }

        public void Delete(int id)
        {
            _stockRepository.ReverseStockForPurchaseReturn(id);
            _repository.Delete(id);
        }

        public bool Exists(int businessId, int id)
        {
            return _repository.Exists(businessId, id);
        }

        public PaginationResponse<PurchaseReturnDto> Get(int businessId, int currentPage, int pageSize, PurchaseQueryParams searchParams)
        {
            var response = _repository.Get(businessId, currentPage, pageSize, searchParams);
            return new PaginationResponse<PurchaseReturnDto>()
            {
                TotalRows = response.TotalRows,
                Rows = response.Rows.Select(pr => MapEntityToDto(pr))
            };
        }

        public IEnumerable<PurchaseReturnDto> Get(int businessId, PurchaseQueryParams searchParams) 
            => _repository.Get(businessId, searchParams).Select(pr => MapEntityToDto(pr));

        public PurchaseReturnDetailsDto? Get(int businessId, int id)
        {
            var purchaseReturn = _repository.Get(businessId, id);
            if(purchaseReturn == null)
                return null;
            return new PurchaseReturnDetailsDto()
            {
                Id = purchaseReturn.Id,
                Date = purchaseReturn.Date,
                PurchaseInvoiceNumber = purchaseReturn.Purchase.InvoiceNumber,
                ReturnInvoiceNumber = purchaseReturn.InvoiceNumber,
                StoreId = purchaseReturn.Purchase.StoreId,
                StoreName = purchaseReturn.Purchase.Store.Name,
                SupplierId = purchaseReturn.Purchase.SupplierId,
                SupplierName = purchaseReturn.Purchase.Supplier.Head.Name,
                PurchaseReturnBy = purchaseReturn.CreatedBy,
                ReturnBill = purchaseReturn.PurchaseReturnProducts.Sum(x => x.GetNetAmount()),
                SupID = purchaseReturn.Purchase.Supplier.SupId,
                SupplierAddress = purchaseReturn.Purchase.Supplier.Address,
                SupplierEmail = purchaseReturn.Purchase.Supplier.Email,
                SupplierMobile = purchaseReturn.Purchase.Supplier.Mobile,
                PreviousDue = purchaseReturn.PreviousDue,
                PurchaseId = purchaseReturn.PurchaseId,
                Products = purchaseReturn.PurchaseReturnProducts.Select(x => new PurchaseReturnProductDto()
                {
                    Id = x.Id,
                    Product = x.PurchaseProduct.Product.Head.Name,
                    PurchaseProductId = x.PurchaseProductId,
                    Qty = x.Qty,
                    UnitVariation = x.PurchaseProduct.UnitVariation.Name,
                    TradePrice = x.PurchaseProduct.TradePrice,
                    Vat = x.PurchaseProduct.Vat,
                    TotalTradePrice = x.GetTotalTradePrice(),
                    TotalVat = x.GetTotalVat(),
                    NetAmount = x.GetNetAmount()
                }).ToList(),
            };
        }

        public void Update(int id, PurchaseReturnViewModel entity)
        {
            var purchaseReturn = new PurchaseReturn(entity.PurchaseId, "", entity.Date, "", "")
            {
                PreviousDue = entity.PreviousDue,
                PurchaseReturnProducts = entity.Products.Select(pr => new PurchaseReturnProduct()
                {
                    PurchaseReturnId = id,
                    PurchaseProductId = pr.PurchaseProductId,
                    Qty = pr.Qty
                }).ToList()
            };

            _stockRepository.ReverseStockForPurchaseReturn(id);
            _repository.Update(id, purchaseReturn);
        }

        private PurchaseReturnDto MapEntityToDto(PurchaseReturn purchaseReturn)
        {
            return new PurchaseReturnDto
            {
                Id = purchaseReturn.Id,
                Date = purchaseReturn.Date,
                PurchaseInvoiceNumber = purchaseReturn.Purchase.InvoiceNumber,
                ReturnInvoiceNumber = purchaseReturn.InvoiceNumber,
                StoreId = purchaseReturn.Purchase.StoreId,
                StoreName = purchaseReturn.Purchase.Store.Name,
                SupplierId = purchaseReturn.Purchase.SupplierId,
                SupplierName = purchaseReturn.Purchase.Supplier.Head.Name,
                ReturnBill = purchaseReturn.PurchaseReturnProducts.Sum(x => x.GetNetAmount()),
            };
        }
    }
}
