using Microsoft.AspNetCore.Mvc;
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
        [Column("username")]
        public virtual string Username { get; set; }

        [Required]
        [StringLength(20)]
        [Column("password")]
        public virtual string Password { get; set; }

        [Required]
        [Column("role")]
        public virtual Role UserRole { get; set; }

        [Required]
        [StringLength(20)]
        [Column("first_name")]
        public virtual string FirstName { get; set; }

        [Required]
        [StringLength(20)]
        [Column("last_name")]
        public virtual string LastName { get; set; }
    }
}
