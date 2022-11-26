using smartshop.Entities.PurchaseManagement;
using smartshop.Entities.SalesManagement;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartshop.Entities.Accounting
{
    public class BankAccount
    {
        [Key]
        [ForeignKey(nameof(Head))]
        public int Id { get; set; }
        public virtual Head? Head { get; set; }

        [Required]
        [StringLength(255)]
        public string BranchName { get; set; }

        [Required, StringLength(50)]
        public string AccountNumber { get; set; }

        [Required]
        public decimal Balance { get; set; }

        public virtual ICollection<Purchase>? Purchases { get; set; }
        public virtual ICollection<Sale>? Sales { get; set; }
        public virtual ICollection<DueCollection>? DueCollections { get; set; }
        public virtual ICollection<DuePayment>? DuePayments { get; set; }
        public virtual ICollection<InvestorTransaction> Transactions { get; set; }
        public virtual ICollection<IncomeExpense> IncomeExpenses { get; set; }

        public BankAccount(int id, string branchName, string accountNumber, decimal balance)
        {
            Id = id;
            BranchName = branchName;
            AccountNumber = accountNumber;
            Balance = balance;
        }

        public BankAccount(Head head, string branchName, string accountNumber, decimal balance = 0)
        {
            Head = head;
            BranchName = branchName;
            AccountNumber = accountNumber;
            Balance = balance;
        }
    }
}
