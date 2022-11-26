using System.ComponentModel.DataAnnotations;

namespace smartshop.Entities.Accounting
{
    public class AccountTransaction
    {
        public int Id { get; set; }

        public int HeadTransactionId { get; set; }
        public virtual HeadTransaction? HeadTransaction { get; set; }

        public int AccountId { get; set; }
        public virtual Account? Account { get; set; }

        [Required]
        [StringLength(1)]
        public string Operator { get; set; }
      
    }
}
