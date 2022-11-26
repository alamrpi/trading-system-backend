namespace smartshop.Business.ViewModels
{
    public class ApplicationUserViewModel
    {
        [Required]
        [StringLength(255)]
        public string FirstName { get; set; }

        [StringLength(255)]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get;  set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        public int StoreId { get; set; }
    }
}
