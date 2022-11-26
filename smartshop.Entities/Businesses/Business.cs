using smartshop.Entities.Accounting;
using smartshop.Entities.HumanResource;
using smartshop.Entities.ProductManagement;
using smartshop.Entities.Settings;
using System.ComponentModel.DataAnnotations;

namespace smartshop.Entities.Businesses
{
    public class Business
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string? Objective { get; set; }

        [Required]
        [MinLength(11)]
        [MaxLength(20)]
        public string ContactNo { get; set; }

        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string WebAddress { get; set; }

        public string? Logo { get; set; }

        public string? LogoPublicId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Store> Stores { get; set; }
        public virtual ICollection<BusinessDeactive> BusinessDeactives{ get; set; }
        public virtual ICollection<Designation>? Designations{ get; set; }
        public virtual ICollection<Head>? Heads{ get; set; }
        public virtual ICollection<Unit>? Units{ get; set; }
        public virtual ICollection<Brand>? Brands{ get; set; }
        public virtual ICollection<Group>? Groups{ get; set; }
        public virtual BusinessConfigure? BusinessConfigure { get; set; }

        public Business(string name, string contactNo, string email, string address, string webAddress, string? objective = null)
        {
            Name = name;
            Objective = objective;
            ContactNo = contactNo;
            Email = email;
            Address = address;
            WebAddress = webAddress;
            CreatedAt = DateTime.UtcNow;
            Stores = new List<Store>();
            BusinessDeactives = new List<BusinessDeactive>();
        }
    }
}
