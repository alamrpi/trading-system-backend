namespace smartshop.Business.Dtos
{
    public class SaleReturnProductDto
    {
        public int Id { get; set; }
        public int saleProductId { get; set; }
        public string Product { get; set; }
        public string UnitVariation { get; set; }
        public decimal Qty { get; set; }
        public decimal Price { get; set; }
        public decimal NetAmount { get; set; }
    }
}
