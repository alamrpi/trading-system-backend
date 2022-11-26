namespace smartshop.Entities.SalesManagement
{
    public class SaleReturnProduct
    {
        public int Id { get; set; }

        public int SaleReturnId { get; set; }
        public virtual SaleReturn? SaleReturn { get; set; }

        public int SaleProductId { get; set; }
        public virtual SaleProduct? SaleProduct { get; set; }

        public decimal Qty { get; set; }

        public decimal GetNetAmount()
        {
            return Qty * SaleProduct.Price;
        }
    }
}
