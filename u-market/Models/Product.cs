using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace u_market.Models
{
    public class Product
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("name")]
        public string Name { get; set; }
        [Required]
        [Column("price")]
        public double Price { get; set; }
        [Required]
        [Column("description")]
        public string Description { get; set; }
        [ForeignKey("store_id")]
        public Store Store { get; set; }
        [Column("image_url")]
        public string ImageUrl { get; set; }
        [Column("video_url")]
        public string VideoUrl { get; set; }
    }
}