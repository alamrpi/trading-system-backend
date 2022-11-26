using smartshop.Common.Enums.Accounting;
using smartshop.Entities.Businesses;
using smartshop.Entities.ProductManagement;
using smartshop.Entities.PurchaseManagement;
using smartshop.Entities.SalesManagement;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartshop.Entities.Accounting
{
    public class Head
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Business))]
        public int BusinessId { get; set; }
        public virtual Business? Business { get; set; }

        [Required, StringLength(255)]
        public string Name { get; set; }

        [Required]
        public HeadTypes HeadType { get; set; }

        [StringLength(255)]
        public string Descriptions { get; set; }

        public bool IsConstant { get; set; }

        public virtual Product? Product { get; set; }
        public virtual BankAccount? BankAccount { get; set; }
        public virtual Supplier? Supplier { get; set; }
        public virtual Customer? Customer { get; set; }

        public virtual ICollection<HeadTransaction>? HeadTransactions { get; set; }
        public virtual ICollection<IncomeExpense> IncomeExpenses { get; set; }

        public Head(int businessId, string name, string descriptions, HeadTypes headType = HeadTypes.General, bool isConstant = true)
        {
            BusinessId = businessId;
            Name = name;
            Descriptions = descriptions;
            HeadType = headType;
            IsConstant = isConstant;
        }
    }
}
