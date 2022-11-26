namespace smartshop.Business.ViewModels
{
    public class PurchaseProductViewModel
    {

        public int ProductId { get; set; }

        public int UnitVariationId { get; set; }

        public decimal Qty { get; set; }
        public decimal BonusQty { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal Vat { get; set; }
        public decimal TradePrice { get; set; }
    }
}
