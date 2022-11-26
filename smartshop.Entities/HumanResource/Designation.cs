using smartshop.Entities.Businesses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartshop.Entities.HumanResource
{
    public class Designation
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Business))]
        public int BusinessId { get; set; }
        public virtual Business? Business { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; }

        [StringLength(255)]
        public string? Descriptions { get; set; }

        [Required]
        public int Priority { get; set; }

        public virtual ICollection<Employee>? Employees { get; set; }

        public Designation(int businessId, string name, int priority = 0, string? descriptions = null)
        {
            BusinessId = businessId;
            Name = name;
            Priority = priority;
            Descriptions = descriptions;
        }
    }
}
