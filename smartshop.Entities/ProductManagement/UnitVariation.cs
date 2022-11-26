using smartshop.Entities.PurchaseManagement;
using smartshop.Entities.SalesManagement;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartshop.Entities.ProductManagement
{
    public class UnitVariation
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Unit))]
        public int UnitId { get; set; }
        public virtual Unit? Unit { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required]
        public double Qnty { get; set; }

        public virtual ICollection<PurchaseProduct>? PurchaseProducts { get; set; }
        public virtual ICollection<SaleProduct>? SaleProducts { get; set; }
        public UnitVariation(string name, double qnty)
        {
            Name = name;
            Qnty = qnty;
        }

        public UnitVariation(string name, double qnty, int unitId) : this(name, qnty)
        {
            UnitId = unitId;
        }
    }
}
