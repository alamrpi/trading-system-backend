using smartshop.Common.Enums.Accounting;
using System.ComponentModel.DataAnnotations;

namespace smartshop.Entities.Accounting
{
    public class InvestorTransaction
    {
        public int Id { get; set; }

        public int InvestorId { get; set; }
        public virtual Investor Investor { get; set; }

        public DateTime Date { get; set; }

        public int BankAccountId { get; set; }
        public virtual BankAccount BankAccount { get; set; }

        public TransactionType TransactionType { get; set; }

        public decimal Amount { get; set; }

        public string Descriptions { get; set; }

        [Required]
        public string CreatedBy { get; set; }

    }
}
