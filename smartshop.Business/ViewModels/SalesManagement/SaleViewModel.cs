namespace smartshop.Business.ViewModels
{
    public class SaleViewModel
    {
        public int CustomerId { get; set; }
   
        public int StoreId { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public decimal Vat { get; set; }

        [Required]
        public decimal Discount { get; set; }

        [StringLength(20)]
        public string? DiscountType { get; set; }

        [Required]
        public decimal Overhead { get; set; }

        [Required]
        public decimal Paid { get; set; }

        public int BankAccountId { get; set; }

        public List<SaleProductViewModel> Products { get; set; }
    }
}
