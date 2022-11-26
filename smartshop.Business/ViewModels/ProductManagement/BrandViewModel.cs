namespace smartshop.Business.ViewModels
{
    public class BrandViewModel
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(255)]
        public string? Comments { get; set; }
    }
}
