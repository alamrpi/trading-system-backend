using smartshop.Entities.ProductManagement;
using smartshop.Entities.Stocks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartshop.Entities.SalesManagement
{
    public class SaleProduct
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Sale))]
        public int SaleId { get; set; }
        public virtual Sale? Sale { get; set; }

        [ForeignKey(nameof(Stock))]
        public int StockId { get; set; }
        public virtual Stock? Stock { get; set; }

        [ForeignKey(nameof(UnitVariation))]
        public int UnitVariationId { get; set; }
        public virtual UnitVariation? UnitVariation { get; set; }

        [Required]
        public decimal Qnty { get; set; }

        [Required]
        public decimal Price { get; set; }

        public virtual ICollection<SaleReturnProduct>? SaleReturnProducts { get; set; }

        public decimal GetTotalAmount() 
          => Qnty * Price;
    }
}
