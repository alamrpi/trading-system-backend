namespace smartshop.Business.Dtos
{
    public class StockDto : StockOrDamageDto
    {
        public decimal Stock { get; set; }
        public decimal TradePrice { get; set; }
    }
}
