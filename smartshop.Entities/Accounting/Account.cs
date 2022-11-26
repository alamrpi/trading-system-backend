using smartshop.Common.Enums.Accounting;
using System.ComponentModel.DataAnnotations;

namespace smartshop.Entities.Accounting
{
    public class Account
    {
        public int Id { get; set; }

        [Required] 
        [StringLength(255)]
        public string Name { get; set; }

        public EquationComponent EquationComponent { get; set; }

        public virtual ICollection<AccountTransaction>? AccountTransactions { get; set; }
        public Account(int id, string name, EquationComponent equationComponent)
        {
            Id = id;
            Name = name;
            EquationComponent = equationComponent;
        }
    }
}
