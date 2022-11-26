using smartshop.Entities.SalesManagement;
using System.ComponentModel.DataAnnotations;

namespace smartshop.Entities.Accounting
{
    public class DueCollection
    {
        public int Id { get; set; }

        public int SaleId { get; set; }
        public virtual Sale? Sale { get; set; }

        public int BankAccountId { get; set; }
        public virtual BankAccount? BankAccount { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public decimal Discount { get; set; }

        public string? DiscountType { get; set; }

        [Required, StringLength(255)]
        public string CreatedBy { get; set; }

        [Required, StringLength(100)]
        public string CreatedIp { get; set; }

        public string SlipNumber { get; set; }

        public decimal PreviouseDue { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        public DueCollection(int saleId, int bankAccountId, DateTime date, decimal amount, decimal discount, string? discountType, string slipNumber, string createdBy, string createdIp)
        {
            SaleId = saleId;
            BankAccountId = bankAccountId;
            Date = date;
            Amount = amount;
            Discount = discount;
            DiscountType = discountType;
            CreatedBy = createdBy;
            CreatedIp = createdIp;
            SlipNumber = slipNumber;
            CreatedAt = DateTime.UtcNow;
        }
        public DueCollection()
        {
            
        }

        public decimal GetDiscountAmount()
        {
            if (DiscountType == "%")
                return (Amount * Discount) / 100;
            else
                return Discount;
        }
    }
}
