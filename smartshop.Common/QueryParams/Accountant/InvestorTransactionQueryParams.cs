using smartshop.Common.Enums.Accounting;

namespace smartshop.Common.QueryParams
{
    public class InvestorTransactionQueryParams : PaginateQueryParams
    {
        public int? InvestorId { get; set; }
        public TransactionType? TransactionType { get; set; }
        public DateTime? Date { get; set; }
    }
}
