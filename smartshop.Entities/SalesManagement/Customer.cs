using smartshop.Common.Enums.SalesManagement;
using smartshop.Entities.Accounting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartshop.Entities.SalesManagement
{
    public class Customer
    {
        [Key]
        [ForeignKey(nameof(Head))]
        public int Id { get; set; }
        public virtual Head? Head { get; set; }

        [Required, StringLength(100)]
        public string Identifier { get; set; }

        [Required, StringLength(20)]
        public string Mobile { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        public CustomerTypes Type { get; set; }

        public virtual ICollection<Sale>? Sales { get; set; }
        public Customer(string identifier, string mobile, string? email, string? address, CustomerTypes type = CustomerTypes.General)
        {
            Identifier = identifier;
            Mobile = mobile;
            Email = email;
            Type = type;
            Address = address;
        }
        public Customer()
        {

        }
    }
}
