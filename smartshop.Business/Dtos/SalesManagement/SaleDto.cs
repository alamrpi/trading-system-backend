namespace smartshop.Business.Dtos
{
    public class SaleDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public int BankAccountId { get; set; }
        public string BankAccountName { get; set; }
        public DateTime Date { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal Paid { get; set; }
        public decimal Due { get; set; }
    }
}
