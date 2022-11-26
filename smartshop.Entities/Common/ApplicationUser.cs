using Microsoft.AspNetCore.Identity;
using smartshop.Entities.HumanResource;
using System.ComponentModel.DataAnnotations;

namespace smartshop.Entities.Common
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser(string firstName, string? lastName, string userName, string email, string phoneNumber, bool isAdmin)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;
            IsAdmin = isAdmin;
            Employees = new List<Employee>();
        }
        public ApplicationUser(string firstName, string? lastName, string userName, string email, string phoneNumber, string photoUrl, string photoPublicId, bool isAdmin) 
            : this(firstName, lastName, userName, email, phoneNumber, isAdmin)
        {
            
            PhotoUrl = photoUrl;
            PhotoPublicId = photoPublicId;
        }
        public string FirstName { get; set; }
        public string? LastName { get; set; }

        public string? PhotoUrl { get; set; }

        public string? PhotoPublicId { get; set; }

        [Required]
        public bool IsAdmin { get; set; } = false;

        [Required]
        public bool IsActive { get; set; } = true;

        public virtual EmployeePersonalInfo? EmployeePersonalInfo { get; set; }
        public virtual ICollection<Employee>? Employees { get; set; }
    }
}
