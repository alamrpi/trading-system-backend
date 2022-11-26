using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartshop.Entities.Accounting
{
    public class IncomeExpense
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Head))]
        public int HeadId { get; set; }
        public virtual Head Head { get; set; }

        [ForeignKey(nameof(BankAccount))]
        public int BankAccountId { get; set; }
        public virtual BankAccount BankAccount { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        public bool Income { get; set; }
    }
}
