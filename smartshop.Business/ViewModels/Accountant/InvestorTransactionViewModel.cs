using smartshop.Common.Enums.Accounting;

namespace smartshop.Business.ViewModels
{
    public class InvestorTransactionViewModel
    {
        public int InvestorId { get; set; }

        public DateTime Date { get; set; }

        public int BankAccountId { get; set; }

        public TransactionType TransactionType { get; set; }

        public decimal Amount { get; set; }

        public string Descriptions { get; set; }

    }
}
