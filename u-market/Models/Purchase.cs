using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace u_market.Models
{
    public class Purchase
    {
        [Key, Column(Order = 0)]
        [ForeignKey("username")]
        public string Username { get; set; }
        [ForeignKey("product_id")]
        [Key, Column(Order = 1)]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
        [Column("time_stamp")]
        public DateTime PurchaseDate { get; set; }

        public Purchase() {
            this.PurchaseDate = DateTime.UtcNow;
        }
    }
}
