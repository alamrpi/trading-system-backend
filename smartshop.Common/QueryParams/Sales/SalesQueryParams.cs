namespace smartshop.Common.QueryParams
{
    public class SalesQueryParams
    {
        public int? CustomerId { get; set; }
        public int? StoreId { get; set; }
        public DateTime? Date { get; set; }
        public string? InvoiceNumber { get; set; }
        public int? BankAccountId { get; set; }

    }
}
