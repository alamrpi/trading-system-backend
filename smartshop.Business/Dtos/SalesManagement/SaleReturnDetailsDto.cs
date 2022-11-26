namespace smartshop.Business.Dtos
{
    public class SaleReturnDetailsDto : SaleReturnDto
    {
        public string CustomerMobile { get; set; }
        public string CustomerIdentifier { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerAddress { get; set; }
        public int SaleId { get; set; }
        public decimal PreviousDue { get; set; }
        public string PurchaseReturnBy { get; set; }

        public List<SaleReturnProductDto> Products { get; set; }
    }
}
