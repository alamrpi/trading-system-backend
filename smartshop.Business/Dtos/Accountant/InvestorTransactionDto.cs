using smartshop.Common.Enums.Accounting;
using smartshop.Entities.Accounting;

namespace smartshop.Business.Dtos.Accountant
{
    public class InvestorTransactionDto
    {
        public int Id { get; set; }

        public int InvestorId { get; set; }
        public string InvestorName { get; set; }

        public DateTime Date { get; set; }

        public int BankAccountId { get; set; }
        public string BankAccountName { get; set; }

        public TransactionType TransactionType { get; set; }

        public decimal Amount { get; set; }

        public string Descriptions { get; set; }

        public string CreatedBy { get; set; }
    }
}
