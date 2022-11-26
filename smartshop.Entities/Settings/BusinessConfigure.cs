using smartshop.Entities.Businesses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartshop.Entities.Settings
{
    public class BusinessConfigure
    {
        [Key]
        [ForeignKey(nameof(Business))]
        public int Id { get; set; }
        public virtual Business? Business { get; set; }

        [Required, StringLength(20)]
        public string CustomerIdPrefix { get; set; }

        [Required, StringLength(20)]
        public string SupplierIdPrefix { get; set; }

        [Required, StringLength(20)]
        public string PurchaseInvoicePrefix { get; set; }

        [Required, StringLength(20)]
        public string SalesInvoicePrefix { get; set; }

        [Required, StringLength(20)]
        public string DuePaymentPrefix { get; set; }

        [Required, StringLength(20)]
        public string DueCollectionPrefix { get; set; }

        public BusinessConfigure(int id, string customerIdPrefix, string supplierIdPrefix, 
            string purchaseInvoicePrefix, string salesInvoicePrefix, string duePaymentPrefix, string dueCollectionPrefix)
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
