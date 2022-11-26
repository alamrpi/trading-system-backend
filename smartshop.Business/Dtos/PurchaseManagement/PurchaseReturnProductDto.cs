namespace smartshop.Business.Dtos
{
    public class PurchaseReturnProductDto
    {
        public int Id { get; set; }
        public string Product { get; set; }
        public int PurchaseProductId { get; set; }
        public string UnitVariation { get; set; }
        public decimal Qty { get; set; }
        public decimal TradePrice { get; set; }
        public decimal Vat { get; set; }
        public decimal TotalVat { get; set; }
        public decimal TotalTradePrice { get; set; }
        public decimal NetAmount { get; set; }
    }
}
