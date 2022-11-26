using smartshop.Entities.Accounting;
using smartshop.Entities.Businesses;
using smartshop.Entities.PurchaseManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartshop.Entities
{
    public class Purchase
    {
        public int Id { get; set; }

        public int SupplierId { get; set; }
        public virtual Supplier? Supplier { get; set; }

        public int StoreId { get; set; }
        public virtual Store? Store { get; set; }

        public int BankAccountId { get; set; }
        public virtual BankAccount? BankAccount { get; set; }

        [Required]
        [StringLength(50)]
        public string InvoiceNumber { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal Discount { get; set; }

        [StringLength(20)]
        public string? DiscountType { get; set; }

        [Required]
        public decimal Overhead { get; set; }

        [Required]
        public decimal Paid { get; set; }

        [Required, StringLength(100)]
        public string CreatedIp { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual ICollection<PurchaseProduct>? PurchaseProducts { get; set; }
        public virtual ICollection<PurchaseReturn>? PurchaseReturns { get; set; }
        public virtual ICollection<DuePayment>? DuePayments { get; set; }

        public Purchase( int supplierId, int storeId, int bankAccountId, string invoiceNumber, DateTime date, decimal discount, string? discountType, decimal overhead, decimal paid, string createdIp, string createdBy)
        {
            SupplierId = supplierId;
            StoreId = storeId;
            BankAccountId = bankAccountId;
            InvoiceNumber = invoiceNumber;
            Date = date;
            Discount = discount;
            DiscountType = discountType;
            Overhead = overhead;
            Paid = paid;
            CreatedIp = createdIp;
            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
        }

        public decimal GetDiscountAmount()
        {
            if (DiscountType == "%")
                return (Discount * GetGrossTradePrice()) / 100;
            else
                return Discount;
        }

        public decimal GetDue() 
            => GetPayableAmount() - Paid;

        public decimal GetGrossTradePrice() 
            => PurchaseProducts.Sum(x => x.GetTotalPurchsePrice());

        public decimal GetGrossVat() 
            => PurchaseProducts.Sum(x => x.GetTotalVat());

        public decimal GetPayableAmount() 
            => (GetGrossTradePrice() + GetGrossVat() + Overhead) - GetDiscountAmount();

        public decimal GetDuePaidAmount()
            => DuePayments.Select(x => x.Amount).Sum();

        public decimal GetCurrentDue() 
            => GetDue() - GetDuePaidAmount();
    }
}
