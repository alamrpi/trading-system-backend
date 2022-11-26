using smartshop.Entities.Businesses;
using smartshop.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartshop.Entities.HumanResource
{
    public class Employee
    {
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }

        [ForeignKey(nameof(Store))]
        public int StoreId { get; set; }
        public virtual Store? Store { get; set; }

        [ForeignKey(nameof(Designation))]
        public int DesignationId { get; set; }
        public virtual Designation? Designation { get; set; }

        [Required]
        public DateTime JoiningDate { get; set; }
        public DateTime? ResignDate { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public string CreatedIp { get; set; }
        [Required]
        public string CreatedBy { get; set; }

        public virtual ICollection<EmployeeSalaryReview>? EmployeeSalaryReviews { get; set; }

        public Employee(string userId, int storeId, int designationId, DateTime joiningDate, string createdIp, string createdBy)
        {
            UserId = userId;
            StoreId = storeId;
            DesignationId = designationId;
            JoiningDate = joiningDate;
            CreatedIp = createdIp;
            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
        }
    }
} 
