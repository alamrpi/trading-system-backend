namespace smartshop.Business.ViewModels
{
    public class DesignationViewModel
    {

        [Required, StringLength(150)]
        public string Name { get; set; }

        [StringLength(255)]
        public string? Descriptions { get; set; }

        [Required]
        public int Priority { get; set; }
    }
}
