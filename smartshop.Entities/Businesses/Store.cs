using smartshop.Entities.HumanResource;
using smartshop.Entities.PurchaseManagement;
using smartshop.Entities.SalesManagement;
using smartshop.Entities.Stocks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartshop.Entities.Businesses
{
    public class Store
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Business))]
        public int BusinessId { get; set; }
        public virtual Business? Business { get; set; }

        [Required, MinLength(5), MaxLength(100)]
        public string Name { get; set; }

        [Required, MinLength(11), MaxLength(20)]
        public string ContactNo { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [Required, MaxLength(10)]
        public string Code { get; set; }

        public virtual ICollection<StoreDeactive>? StoreDeactives { get; set; }
        public virtual ICollection<Employee>? Employees { get; set; }
        public virtual ICollection<Stock>? Stocks { get; set; }
        public virtual ICollection<Purchase>? Purchases { get; set; }
        public virtual ICollection<Sale>? Sales { get; set; }
        public Store(int businessId, string name, string contactNo, string email, string address, string code)
        {
            BusinessId = businessId;
            Name = name;
            ContactNo = contactNo;
            Email = email;
            Address = address;
            Code = code;
        }
    }
}
