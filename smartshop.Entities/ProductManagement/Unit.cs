using smartshop.Entities.Businesses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartshop.Entities.ProductManagement
{
    public class Unit
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Business))]
        public int BusinessId { get; set; }
        public virtual Business? Business { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string Symbol { get; set; }

        public string? Comments { get; set; }

        public virtual ICollection<UnitVariation>? UnitVariations { get; set; }
        public virtual ICollection<Product>? Products { get; set; }

        public Unit(int businessId, string name, string symbol, string comments)
        {
            BusinessId = businessId;
            Name = name;
            Symbol = symbol;
            Comments = comments;
        }
    }
}
