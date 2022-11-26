namespace smartshop.Business.ViewModels.ProductManagement
{
    public class UnitViewModel
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string Symbol { get; set; }

        public string? Comments { get; set; }

    }
}
