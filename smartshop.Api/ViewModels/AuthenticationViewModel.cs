using System.ComponentModel.DataAnnotations;

namespace smartshop.Api.ViewModels
{
    public class AuthenticationViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
