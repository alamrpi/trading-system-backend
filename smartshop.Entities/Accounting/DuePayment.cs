using System.ComponentModel.DataAnnotations;

namespace smartshop.Entities.Accounting
{
    public class DuePayment
    {
        public int Id { get; set; }

        public int PurchaseId { get; set; }
        public virtual Purchase? Purchase { get; set; }

        public int BankAccountId { get; set; }
        public virtual BankAccount BankAccount { get; set; }

        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }

        [StringLength(10)]
        public string DiscountType { get; set; }

        public decimal PreviousDue { get; set; }

        [Required, StringLength(50)]
        public string PaymentSlipNumber { get; set; }

        [Required, StringLength(100)]
        public string CreatedIp { get; set; }

        [Required]
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DuePayment(int purchaseId, DateTime date, decimal amount, decimal discount, string discountType, string paymentSlipNumber, string createdIp, string createdBy)
        {
            PurchaseId = purchaseId;
            Date = date;
            Amount = amount;
            Discount = discount;
            DiscountType = discountType;
            PaymentSlipNumber = paymentSlipNumber;
            CreatedIp = createdIp;
            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
        }
        public DuePayment()
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
