using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartshop.Entities.Businesses
{
    public class BusinessDeactive
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Business))]
        public int BusinessId { get; set; }
        public virtual Business? Business { get; set; }

        [Required]
        public string Descriptions { get; set; }

        [Required]
        public DateTime DeactiveDate { get; set; }

        public DateTime? ReActivateDate { get; set; }

        public BusinessDeactive(int businessId, string descriptions)
        {
            BusinessId = businessId;
            Descriptions = descriptions;
            DeactiveDate = DateTime.UtcNow;
        }
    }
}
