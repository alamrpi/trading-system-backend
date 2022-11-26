namespace smartshop.Business.Dtos
{
    public class BusinessConfigureDto
    { 
        public int Id { get; set; }
        public string CustomerIdPrefix { get; set; }
    
        public string SupplierIdPrefix { get; set; }

        public string PurchaseInvoicePrefix { get; set; }

        public string SalesInvoicePrefix { get; set; }
        public string DuePaymentPrefix { get; set; }
        public string DueCollectionPrefix { get; set; }
        public BusinessConfigureDto(int id, string customerIdPrefix, string supplierIdPrefix, string purchaseInvoicePrefix, string salesInvoicePrefix, string duePaymentPrefix, string dueCollectionPrefix)
        {
            Id = id;
            CustomerIdPrefix = customerIdPrefix;
            SupplierIdPrefix = supplierIdPrefix;
            PurchaseInvoicePrefix = purchaseInvoicePrefix;
            SalesInvoicePrefix = salesInvoicePrefix;
            DuePaymentPrefix = duePaymentPrefix;
            DueCollectionPrefix = dueCollectionPrefix;
        }
    }
}
