namespace smartshop.Business.Dtos
{
    public class PurchaseProductDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public int UnitVariationId { get; set; }
        public string UnitVariation { get; set; }

        public decimal Qty { get; set; }
        public decimal BonusQty { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal Vat { get; set; }
        public decimal TradePrice { get; set; }

        public decimal TotalPurchasePrice { get; set; }
        public decimal TotalVat { get; set; }
      
    }
}
