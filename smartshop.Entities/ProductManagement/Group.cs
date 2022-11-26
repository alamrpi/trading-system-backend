using smartshop.Entities.Businesses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartshop.Entities.ProductManagement
{
    public class Group
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Business))]
        public int BusinessId { get; set; }
        public virtual Business? Business { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(255)]
        public string? Comments { get; set; }

        public virtual ICollection<Category>? Categories { get; set; }
        public Group(int businessId, string name, string comments)
        {
            BusinessId = businessId;
            Name = name;
            Comments = comments;
        }
    }
}
