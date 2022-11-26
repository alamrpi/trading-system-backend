namespace smartshop.Business.Dtos
{
    public class PurchaseDetailsDto : PurchaseDto
    {
        public string SupplierEmail { get; set; }
        public string SupplierPhone { get; set; }
        public string SupplierAddress { get; set; }
        public string SupId { get; set; }

        public string StoreContact { get; set; }
        public string StoreEmail { get; set; }
        public string StoreAddress { get; set; }

        public decimal Discount { get; set; }
        public string? DiscountType { get; set; }
        public decimal Overhead { get; set; }

        public decimal GrossTradePrice { get; set; }
        public decimal GrossVat { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal NetPayableAmount { get; set; }
        public decimal Due { get; set; }

        public string CreatedIp { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public IList<PurchaseProductDto> PurchaseProducts { get; set; }
    }
}
