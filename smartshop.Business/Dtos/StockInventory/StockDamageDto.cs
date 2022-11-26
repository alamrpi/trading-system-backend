namespace smartshop.Business.Dtos
{
    public class StockDamageDto : StockOrDamageDto
    {
        public decimal DamageQty { get; set; }
        public string? Descriptions { get; set; }
    }
}
