using smartshop.Common.Enums.Accounting;

namespace smartshop.Common.QueryParams
{
    public class LedgerFilterQueryParams
    {
        public HeadTypes HeadType { get; set; }
        public int? HeadId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
