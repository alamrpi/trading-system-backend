using smartshop.Common.Enums.Accounting;

namespace smartshop.Business.Dtos
{
    public class LedgerDto
    {
        public string HeadName { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public string? Descriptions { get; set; }
        public string Date { get; set; }
    }
}
