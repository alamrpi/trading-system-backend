using smartshop.Entities.SalesManagement;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartshop.Entities
{
    public class SaleReturn
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Sale))]
        public int SaleId { get; set; }
        public virtual Sale? Sale { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required, StringLength(100)]
        public string InvoiceNumber { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public string CreatedIp { get; set; }

        public virtual ICollection<SaleReturnProduct>? SaleReturnProducts { get; set; }
        public decimal PreviousDue { get; set; }

        public SaleReturn(int saleId, DateTime date, string invoiceNumber, string createdBy, string createdIp)
        {
            SaleId = saleId;
            Date = date;
            InvoiceNumber = invoiceNumber;
            CreatedBy = createdBy;
            CreatedIp = createdIp;
        }
    }
}
