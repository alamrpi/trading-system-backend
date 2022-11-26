using smartshop.Entities.Businesses;
using smartshop.Entities.ProductManagement;
using smartshop.Entities.SalesManagement;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartshop.Entities.Stocks
{
    public class Stock
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Store))]
        public int StoreId { get; set; }
        public virtual Store? Store { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }

        [Required]
        public decimal StockQty { get; set; }

        [Required]
        public decimal TradePrice { get; set; }

        public virtual ICollection<StockDamage>? StockDamages { get; set; }
        public virtual ICollection<SaleProduct>? SaleProducts { get; set; }

        public Stock(int storeId, int productId, decimal stockQnty, decimal tradePrice)
        {
            StoreId = storeId;
            ProductId = productId;
            StockQty = stockQnty;
            TradePrice = tradePrice;
        }
        public Stock()
        {

        }
    }
}
