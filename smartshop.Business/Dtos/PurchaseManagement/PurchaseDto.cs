namespace smartshop.Business.Dtos
{
    public class PurchaseDto
    {
        public int Id { get; set; }

        public int SupplierId { get; set; }
        public string SupplierName { get; set; }

        public int StoreId { get; set; }
        public string StoreName { get; set; }

        public int BankAccountId { get; set; }
        public string BankAccountName { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime Date { get; set; }
        public decimal Paid { get; set; }
    }
}
