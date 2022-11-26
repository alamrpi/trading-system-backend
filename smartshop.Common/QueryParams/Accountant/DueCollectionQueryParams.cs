namespace smartshop.Common.QueryParams
{
    public class DueCollectionQueryParams : PaginateQueryParams
    {
        public int? StoreId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? Date { get; set; }
        public string? InvoiceNumber { get; set; }
    }
}
