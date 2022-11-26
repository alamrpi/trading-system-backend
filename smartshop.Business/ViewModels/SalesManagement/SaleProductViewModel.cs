namespace smartshop.Business.ViewModels
{
    public class SaleProductViewModel
    {

        public int StockId { get; set; }

        public int UnitVariationId { get; set; }

        [Required]
        public decimal Qnty { get; set; }

        [Required]
        public decimal Price { get; set; }

    }
}
