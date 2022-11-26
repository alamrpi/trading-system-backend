namespace smartshop.Entities.PurchaseManagement
{
    public class PurchaseReturnProduct
    {
        public int Id { get; set; }

        public int PurchaseReturnId { get; set; }
        public virtual PurchaseReturn? PurchaseReturn { get; set; }

        public int PurchaseProductId { get; set; }
        public virtual PurchaseProduct? PurchaseProduct { get; set; }

        public decimal Qty { get; set; }

        public decimal GetTotalTradePrice()
        {
            return Qty * PurchaseProduct.TradePrice;
        }

        public decimal GetTotalVat()
        {
            return Qty * PurchaseProduct.Vat;
        }

        public decimal GetNetAmount()
        {
           return GetTotalTradePrice() + GetTotalVat();
        }
    }
}
