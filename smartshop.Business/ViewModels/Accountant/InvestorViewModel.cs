namespace smartshop.Business.ViewModels
{
    public class InvestorViewModel
    {
        [Required, StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Descriptions { get; set; }

        [Required]
        [StringLength(11)]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
