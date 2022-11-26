using smartshop.Entities.Accounting;
using smartshop.Entities.PurchaseManagement;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartshop.Entities
{
    public class Supplier
    {
        [Key]
        [ForeignKey(nameof(Head))]
        public int Id { get; set; }
        public virtual Head? Head { get; set; }

        [Required]
        [StringLength(100)]
        public string SupId { get; set; }

        [Required]
        [StringLength(15)]
        public string Mobile { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public string Address { get; set; }

        public virtual ICollection<Purchase> Purchases { get; set; }
        public Supplier(int id, string supId, string mobile, string email, string address)
        {
            Id = id;
            SupId = supId;
            Mobile = mobile;
            Email = email;
            Address = address;
        }
        public Supplier(Head head, string supId, string mobile, string email, string address)
        {
            Head = head;
            SupId = supId;
            Mobile = mobile;
            Email = email;
            Address = address;
        }
    }
}
