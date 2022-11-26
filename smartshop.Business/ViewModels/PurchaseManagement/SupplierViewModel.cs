namespace smartshop.Business.ViewModels
{
    public class SupplierViewModel
    {
        [Required, StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(15)]
        public string Mobile { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public string Address { get; set; }

        [StringLength(255)]
        public string Descriptions { get; set; }

    }
}
