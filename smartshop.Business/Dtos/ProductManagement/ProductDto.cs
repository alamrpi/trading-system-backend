namespace smartshop.Business.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int GroupId { get; set; }
        public string GroupName { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public int BrandId { get; set; }
        public string BrandName { get; set; }

        public int UnitId { get; set; }
        public string UnitName { get; set; }

        public double AlertQty { get; set; }
        public string? Descriptions { get; set; }

        public string Barcode { get; set; }
    }
}
