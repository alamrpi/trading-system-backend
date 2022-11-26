namespace smartshop.Business.Dtos
{
    public class PurchaseReturnDto
    {
        public int Id { get; set; }

        public string PurchaseInvoiceNumber { get; set; }
        public string ReturnInvoiceNumber { get; set; }

        public string SupplierName { get; set; }
        public int SupplierId { get; set; }

        public int StoreId { get; set; }
        public string StoreName { get; set; }

        public DateTime Date { get; set; }
        public decimal ReturnBill { get; set; }
    }
}
