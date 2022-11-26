using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace smartshop.Entities.Accounting
{
    public class Investor
    {
        [Key]
        [ForeignKey(nameof(Head))]
        public int Id { get; set; }
        public virtual Head? Head { get; set; }

        [Required]
        [StringLength(11)]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public virtual ICollection<InvestorTransaction> Transactions { get; set; }
    }
}
