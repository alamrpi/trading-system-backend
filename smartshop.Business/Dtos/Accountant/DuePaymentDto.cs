namespace smartshop.Business.Dtos
{
    public class DuePaymentDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string BankAccountName { get; set; }
        public int BankAccountId { get; set; }
        public string PurchaseInvoiceNumber { get; set; }
        public int PurchaseId { get; set; }
        public string SlipNumber { get; set; }
        public string StoreName { get; set; }
        public int StoreId { get; set; }
        public string SupplierName { get; set; }
        public int SupplierId { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Discount { get; set; }
        public string DiscountType { get; set; }
        public decimal PreviousDue { get; set; }
    }
}
