namespace smartshop.Business.Dtos
{
    public class SaleDetailsDto : SaleDto
    {
        public string CustomerMobile { get; set; }
        public string? CustomerAddress { get; set; }
        public string StoreEmail { get; set; }
        public string StoreContact { get; set; }
        public string StoreAddress { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal Vat { get; set; }
        public decimal VatAmount { get; set; }
        public decimal Discount { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Overhead { get; set; }
        public DateTime EntryAt { get; set; }
        public string EntryBy { get; set; }

        public List<SaleProductDto> Products { get; set; }
    }
}
