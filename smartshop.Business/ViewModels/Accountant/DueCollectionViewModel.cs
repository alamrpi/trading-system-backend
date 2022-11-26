namespace smartshop.Business.ViewModels
{
    public class DueCollectionViewModel
    {
        public int SaleId { get; set; }
        public int BankAccountId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }

        [StringLength(10)]
        public string DiscountType { get; set; }
        public decimal PreviousDue { get; set; }
    }
}
