using smartshop.Common.Enums.SalesManagement;

namespace smartshop.Business.ViewModels
{
    public class CustomerViewModel
    {
        [Required, StringLength(255)]
        public string Name { get; set; }

        [StringLength(500)]
        public string? Descriptions { get; set; }

        [Required, StringLength(20)]
        public string Mobile { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        public CustomerTypes Type { get; set; }
    }
}
