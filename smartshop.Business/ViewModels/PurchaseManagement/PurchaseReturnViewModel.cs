namespace smartshop.Business.ViewModels
{
    public class PurchaseReturnViewModel
    {
        public int PurchaseId { get; set; }

        public DateTime Date { get; set; }
        public decimal PreviousDue { get; set; }

        public List<PurchaseReturnProductViewModel> Products { get; set; }
    }
}
