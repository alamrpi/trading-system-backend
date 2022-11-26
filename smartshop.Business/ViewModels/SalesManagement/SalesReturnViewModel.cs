namespace smartshop.Business.ViewModels
{
    public class SalesReturnViewModel
    {
        public int SaleId { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public decimal PreviousDue { get; set; }

        public List<SaleReturnProductViewModel> Products { get; set; }
    }
}
