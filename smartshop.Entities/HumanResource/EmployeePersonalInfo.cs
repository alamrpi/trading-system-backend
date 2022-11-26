using smartshop.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartshop.Entities.HumanResource
{
    public class EmployeePersonalInfo
    {
        [Key]
        [ForeignKey(nameof(ApplicationUser))]
        public string Id { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required,StringLength(150)]
        public string FatherName { get; set; }

        [Required, StringLength(20)]
        public string NationalIdNumber { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Address { get; set; }

        public EmployeePersonalInfo(string id, string gender, string fatherName, string nationalIdNumber, DateTime dateOfBirth, string address)
        {
            Id = id;
            Gender = gender;
            FatherName = fatherName;
            NationalIdNumber = nationalIdNumber;
            DateOfBirth = dateOfBirth;
            Address = address;
        }
    }
}
