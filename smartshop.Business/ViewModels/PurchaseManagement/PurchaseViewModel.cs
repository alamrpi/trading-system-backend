namespace smartshop.Business.ViewModels
{
    public class PurchaseViewModel
    {
        public int SupplierId { get; set; }

        public int StoreId { get; set; }

        public int BankAccountId { get; set; }

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

        public virtual ICollection<PurchaseProductViewModel> PurchaseProducts { get; set; }
    }
}
