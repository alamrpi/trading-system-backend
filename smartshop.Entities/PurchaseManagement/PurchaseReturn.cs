using smartshop.Entities.PurchaseManagement;
using System.ComponentModel.DataAnnotations;

namespace smartshop.Entities
{
    public class PurchaseReturn
    {
        public int Id { get; set; }

        public int PurchaseId { get; set; }
        public virtual Purchase? Purchase { get; set; }

        [Required, StringLength(100)]
        public string InvoiceNumber { get; set; }

        public DateTime Date { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public string CreatedIp { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public decimal PreviousDue { get; set; }
        public virtual ICollection<PurchaseReturnProduct>? PurchaseReturnProducts { get; set; }

        public PurchaseReturn(int purchaseId, string invoiceNumber, DateTime date, string createdBy, string createdIp)
        {
            PurchaseId = purchaseId;
            InvoiceNumber = invoiceNumber;
            Date = date;
            CreatedBy = createdBy;
            CreatedIp = createdIp;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
