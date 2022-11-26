namespace smartshop.Common.QueryParams
{
    public class DuePaymentQueryParams : PaginateQueryParams
    {
        public int? StoreId { get; set; }
        public int? SupplierId { get; set; }
        public DateTime? Date { get; set; }
        public string? InvoiceNumber { get; set; }
    }
}
