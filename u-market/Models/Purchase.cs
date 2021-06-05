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
        [Key, Column("username",Order = 0)]
        public string Username { get; set; }
        [ForeignKey("Username")]
        public virtual User User { get; set; }
        [Key, Column("product_id",Order = 1)]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [Key, Column("time_stamp", Order = 2)]
        public DateTime PurchaseDate { get; set; }

        public Purchase() {
            this.PurchaseDate = DateTime.UtcNow;
        }
    }
}
