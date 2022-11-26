namespace smartshop.Business.Dtos
{
    public class SaleProductDto
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public string ProductDetails { get; set; }
        public int UnitVariatonId { get; set; }
        public string UnitVariatonName { get; set; }
        public decimal Qnty { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
