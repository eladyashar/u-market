using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace u_market.Models
{
    public class Store
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        
        [Column("lat")]
        public double Lat { get; set; }
        
        [Column("lang")]
        public double Lang { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Required]
        [Column("OwnerId")]
        public string OwnerId { get; set; }
        
        [ForeignKey("OwnerId")]
        public User Owner { get; set; }
        
        public ICollection<Product> Products { get; set; }
    }
}
