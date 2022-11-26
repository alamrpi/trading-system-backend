namespace smartshop.Common.QueryParams.Purchase
{
    public class PurchaseQueryParams
    {
        public int? StoreId { get; set; }
        public int? SupplierId { get; set; }
        public string? InvoiceNumber { get; set; }
        public DateTime? Date { get; set; }
    }
}
