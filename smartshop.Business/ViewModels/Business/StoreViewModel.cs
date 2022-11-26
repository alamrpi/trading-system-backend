namespace smartshop.Business.ViewModels.Business
{
    public class StoreViewModel
    {
        public int? BusinessId { get; set; }

        [Required, MinLength(5), MaxLength(100)]
        public string Name { get; set; }

        [Required, MinLength(11), MaxLength(20)]
        public string ContactNo { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [Required, MaxLength(10)]
        public string Code { get; set; }
    }
}
