using Microsoft.AspNetCore.Http;

namespace smartshop.Business.ViewModels.HumanResources
{
    public class EmployeePersonalInfoViewModel : ApplicationUserViewModel
    {
        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required, StringLength(150)]
        public string FatherName { get; set; }

        [Required, StringLength(20)]
        public string NationalIdNumber { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Address { get; set; }

        public IFormFile? PhotoFile { get; set; }

    }
}
