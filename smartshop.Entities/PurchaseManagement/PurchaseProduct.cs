using smartshop.Entities.ProductManagement;

namespace smartshop.Entities.PurchaseManagement
{
    public class PurchaseProduct
    {
        public int Id { get; set; }

        public int PurchaseId { get; set; }
        public virtual Purchase? Purchase { get; set; }

        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }

        public int UnitVariationId { get; set; }
        public virtual UnitVariation? UnitVariation { get; set; }

        public decimal Qty { get; set; }
        public decimal BonusQty { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal Vat { get; set; }
        public decimal TradePrice { get; set; }

        public virtual ICollection<PurchaseReturnProduct>? PurchaseReturnProducts { get; set; }

        public PurchaseProduct(int productId, int unitVariationId, decimal qty, decimal bonusQty, decimal purchasePrice,
            decimal vat, decimal tradePrice)
        {
              ProductId = productId;
            UnitVariationId = unitVariationId;
            Qty = qty;
            BonusQty = bonusQty;
            PurchasePrice = purchasePrice;
            Vat = vat;
            TradePrice = tradePrice;
        }

        public decimal GetTotalVat()
            => Qty * Vat;

        public decimal GetTotalPurchsePrice() 
            => PurchasePrice * Qty;
    }
}
