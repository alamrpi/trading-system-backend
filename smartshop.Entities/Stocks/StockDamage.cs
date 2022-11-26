using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartshop.Entities.Stocks
{
    public class StockDamage
    {
        public int Id { get; set; }

        [ForeignKey(nameof(StockId))]
        public int StockId { get; set; }
        public virtual Stock? Stock { get; set; }

        public decimal DamageQnty { get; set; }

        [StringLength(500)]
        public string? Descriptions { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required, StringLength(255)]
        public string CreatedIp { get; set; }

        [Required]
        public string CreatedBy { get; set; }
        public StockDamage(int stockId, decimal damageQnty, string? descriptions, string createdIp, string createdBy)
        {
            StockId = stockId;
            DamageQnty = damageQnty;
            Descriptions = descriptions;
            CreatedAt = DateTime.UtcNow;
            CreatedIp = createdIp;
            CreatedBy = createdBy;
        }

        public StockDamage()
        {

        }
    }
}
