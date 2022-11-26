using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartshop.Entities.Businesses
{
    public class StoreDeactive
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Store))]
        public int StoreId { get; set; }
        public virtual Store? Store { get; set; }

        [Required]
        public string Descriptions { get; set; }

        [Required]
        public DateTime DeactiveDate { get; set; }

        public DateTime? ReActivateDate { get; set; }

        public StoreDeactive(int storeId, string descriptions)
        {
            StoreId = storeId;
            Descriptions = descriptions;
            DeactiveDate = DateTime.UtcNow;
        }
    }
}
