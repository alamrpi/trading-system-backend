namespace smartshop.Business.Dtos
{
    public class DueCollectionDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string BankAccountName { get; set; }
        public int BankAccountId { get; set; }
        public string SaleInvoiceNumber { get; set; }
        public int SaleId { get; set; }
        public string SlipNumber { get; set; }
        public string StoreName { get; set; }
        public int StoreId { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public decimal Amount { get; set; }
        public decimal PreviousDue { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Discount { get; set; }
        public string? DiscountType { get; set; }
    }
}
