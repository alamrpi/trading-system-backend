namespace smartshop.Business.ViewModels
{
    public class StockDamageViewModel
    {
        public int StockId { get; set; }

        public decimal DamageQnty { get; set; }

        [StringLength(500)]
        public string? Descriptions { get; set; }

    }
}
