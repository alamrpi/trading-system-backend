using Newtonsoft.Json;

namespace smartshop.Business.ViewModels
{
    public class BusinessConfigureViewModel
    {
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
    }
}
