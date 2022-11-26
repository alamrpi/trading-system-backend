using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace smartshop.Business.ViewModels
{
    public class UpdateProfileViewModel
    {
        [Required, StringLength(255)]
        public string FirstName { get; set; }

        [StringLength(255)]
        public string LastName { get; set; }

        [Required, StringLength(100)]
        public string Email { get; set; }

        [Required, StringLength(11), MinLength(11)]
        public string Phone { get; set; }

        [FileExtensions(Extensions = "jpg,jpeg,png,gif")]
        public IFormFile? ProfilePicture { get; set; }
    }
}
