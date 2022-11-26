using smartshop.Common.Enums.Accounting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartshop.Entities.Accounting
{
    public class HeadTransaction
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Head))]
        public int HeadId { get; set; }
        public virtual Head? Head { get; set; }

        [Required]
        public TransactionType Type { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [StringLength(255)]
        public string? Descriptions { get; set; }

        public int? purchaseId { get; set; }
        public int? purchaseReturnId { get; set; }
        public int? SaleId { get; set; }
        public int? SaleReturnId { get; set; }
        public int? DueCollectionId { get; set; }
        public int? DuePaymentId { get; set; }
        public int? InvestorTransactionId { get; set; }
        public int? IncomeExpenseId { get; set; }
        public virtual ICollection<AccountTransaction> AccountTransactions { get; set; }
      
    }
}
