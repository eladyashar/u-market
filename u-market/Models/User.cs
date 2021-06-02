using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace u_market.Models
{
    public enum Role
    {
        Admin, Client
    }
    public class User
    {
        [Key]
        [Required]
        [StringLength(20)]
        [RegularExpression("([a-zA-Z0-9]+)", ErrorMessage = "Enter only letters and numbers for username")]
        [Column("username")]
        public string Username { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression("([a-zA-Z0-9]+)", ErrorMessage = "Enter only letters and numbers for password")]
        [Column("password")]
        public string Password { get; set; }

        [Required]
        [DefaultValue(Role.Client)]
        [Range(0,2)]
        [Column("role")]
        public Role UserRole { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression("([a-zA-Z]+[a-zA-Z- ]*[a-zA-Z]+)|([a-zA-Z]){1}", ErrorMessage = "Invalid Name")]
        [Column("first_name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression("([a-zA-Z]+[a-zA-Z- ]*[a-zA-Z]+)|([a-zA-Z]){1}", ErrorMessage = "Invalid Name")]
        [Column("last_name")]
        public string LastName { get; set; }
    }
}
