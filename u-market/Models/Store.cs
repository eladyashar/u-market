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
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("lat")]
        public double Lat { get; set; }
        [Column("lang")]
        public double Lang { get; set; }
        [ForeignKey("username")]
        [Required]
        public string OwnerId { get; set; }
        public User Owner { get; set; }
    }
}
