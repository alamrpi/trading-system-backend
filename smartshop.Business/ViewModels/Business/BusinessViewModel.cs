using Microsoft.AspNetCore.Http;
using smartshop.Business.CustomValidations;

namespace smartshop.Business.ViewModels.Business
{
    public class BusinessViewModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string? Objective { get; set; }

        [Required]
        [MinLength(11)]
        [MaxLength(20)]
        public string ContactNo { get; set; }

        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string WebAddress { get; set; }

        [MaxFileSize(1)]
        [AllowFileExtensions(new string[] { ".jpg", ".png", ".jpeg", ".gif" })]
        public IFormFile? Logo { get; set; }
    }
}
