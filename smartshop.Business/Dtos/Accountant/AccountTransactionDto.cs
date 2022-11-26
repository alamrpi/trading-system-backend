using smartshop.Common.Enums.Accounting;

namespace smartshop.Business.Dtos.Accountant
{
    public class AccountTransactionDto
    {
        public string HeadName { get; set; }
        public string AccountName { get; set; }
        public EquationComponent Component { get; set; }
        public decimal Amount { get; set; }
        public string Operator { get; set; }
        public string? Descriptions { get; set; }
        public string Date { get; set; }
    }
}
