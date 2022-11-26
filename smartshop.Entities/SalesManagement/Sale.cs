using smartshop.Entities.Accounting;
using smartshop.Entities.Businesses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartshop.Entities.SalesManagement
{
    public class Sale
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        [ForeignKey(nameof(Store))]
        public int StoreId { get; set; }
        public virtual Store? Store { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required, StringLength(100), MinLength(3)]
        public string InvoiceNumber { get; set; }

        [Required]
        public decimal Vat { get; set; }

        [Required]
        public decimal Discount { get; set; }

        [StringLength(20)]
        public string DiscountType { get; set; }

        [Required]
        public decimal Overhead { get; set; }
        
        [Required]
        public decimal Paid { get; set; }

        [ForeignKey(nameof(BankAccount))]
        public int BankAccountId { get; set; }
        public virtual BankAccount? BankAccount { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public string CreatedIp { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<SaleProduct>? SaleProducts { get; set; }
        public virtual ICollection<SaleReturn>? SaleReturns { get; set; }
        public virtual ICollection<DueCollection>? DueCollections { get; set; }

        public Sale(int customerId, int storeId, DateTime date, string invoiceNumber, decimal vat, decimal discount, string discountType, decimal overhead, decimal paid, int bankAccountId, string createdBy, string createdIp)
        {        
            CustomerId = customerId;
            StoreId = storeId;
            Date = date;
            InvoiceNumber = invoiceNumber;
            Vat = vat;
            Discount = discount;
            DiscountType = discountType;
            Overhead = overhead;
            Paid = paid;
            BankAccountId = bankAccountId;
            CreatedBy = createdBy;
            CreatedIp = createdIp;
        }

        public decimal GrandTotal()
            => SaleProducts.Sum(x => x.GetTotalAmount());

        public decimal GetPayableAmount() 
            => (GrandTotal() + Overhead + GetVatAmount()) - GetDiscountAmount();

        public decimal GetDueAmount() 
            => GetPayableAmount() - Paid;

        public decimal GetDiscountAmount()
        {
            if (DiscountType == "%")
                return (GrandTotal() * Discount) / 100;
            else
                return Discount;
        }
        public decimal GetVatAmount() 
            => (GrandTotal() * Vat) / 100;

        public decimal GetPaidDue()
        {
            return DueCollections.Select(x => x.Amount).Sum();
        }

        public decimal GetCurrentDue()
        {
            return GetDueAmount() - GetPaidDue();
        }
    }
}
