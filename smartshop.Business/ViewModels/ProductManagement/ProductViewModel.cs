namespace smartshop.Business.ViewModels
{
    public class ProductViewModel
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public int UnitId { get; set; }
        public double AlertQty { get; set; }
        public string? Descriptions { get; set; }
    }
}
