namespace smartshop.Business.ViewModels
{
    public class HeadViewModel
    {
        [Required, StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Descriptions { get; set; }
    }
}
