using smartshop.Entities.Accounting;
using smartshop.Entities.PurchaseManagement;
using smartshop.Entities.Stocks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartshop.Entities.ProductManagement
{
    public class Product
    {
        [Key, ForeignKey(nameof(Head))]
        public int Id { get; set; }
        public virtual Head? Head { get; set; }

        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        public int BrandId { get; set; }
        public virtual Brand? Brand { get; set; }

        public int UnitId { get; set; }
        public virtual Unit? Unit { get; set; }

        public double AlertQty { get; set; }

        [Required]
        public string Barcode { get; set; }
        public string? Descriptions { get; set; }

        public virtual ICollection<Stock>? Stocks { get; set; }
        public virtual ICollection<PurchaseProduct>? PurchaseProducts { get; set; }
        public Product(int id, int categoryId, int brandId, int unitId, double alertQty, string barcode, string descriptions)
        {
            Id = id;
            CategoryId = categoryId;
            BrandId = brandId;
            UnitId = unitId;
            AlertQty = alertQty;
            Descriptions = descriptions;
            Barcode = barcode;
        }
        public Product()
        {

        }
    }
}
