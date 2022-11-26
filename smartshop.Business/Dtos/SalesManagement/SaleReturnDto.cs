namespace smartshop.Business.Dtos
{
    public class SaleReturnDto
    {
        public int Id { get; set; }

        public string SaleInvoiceNumber { get; set; }
        public string ReturnInvoiceNumber { get; set; }

        public string CustomerName { get; set; }
        public int CustomerId { get; set; }

        public int StoreId { get; set; }
        public string StoreName { get; set; }

        public DateTime Date { get; set; }
        public decimal ReturnBill { get; set; }
    }
}
