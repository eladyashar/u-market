using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace u_market.Models
{
    public class Tag
    {
        [Key]
        [Column("tag_id")]
        public int Id { get; set; }

        [Column("name")]
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
