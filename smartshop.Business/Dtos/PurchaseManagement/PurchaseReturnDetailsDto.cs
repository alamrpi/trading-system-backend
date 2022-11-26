namespace smartshop.Business.Dtos
{
    public class PurchaseReturnDetailsDto : PurchaseReturnDto
    {
        public int PurchaseId { get; set; }
        public string SupplierMobile { get; set; }
        public string SupID { get; set; }
        public string SupplierEmail { get; set; }
        public string SupplierAddress { get; set; }

        public string PurchaseReturnBy { get; set; }
        public decimal PreviousDue { get; set; }

        public List<PurchaseReturnProductDto> Products { get; set; }
    }
}
