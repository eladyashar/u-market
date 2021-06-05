using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace u_market.Models
{
    public class AddUser : User
    {
        [RegularExpression("([a-zA-Z0-9]+)", ErrorMessage = "Enter only letters and numbers for username")]
        [StringLength(20)]
        [Remote("OnPostUserChecking", "Users", ErrorMessage="Name taken")]
        public override string Username { get; set; }

        [RegularExpression("([a-zA-Z0-9]+)", ErrorMessage = "Enter only letters and numbers for password")]
        [StringLength(20)]
        public override string Password { get; set; }

        [DefaultValue(Role.Client)]
        public override Role UserRole { get; set; }

        [RegularExpression("([a-zA-Z]+[a-zA-Z- ]*[a-zA-Z]+)|([a-zA-Z]){1}", ErrorMessage = "Invalid Name")]
        [StringLength(20)]
        public override string FirstName { get; set; }

        [RegularExpression("([a-zA-Z]+[a-zA-Z- ]*[a-zA-Z]+)|([a-zA-Z]){1}", ErrorMessage = "Invalid Name")]
        [StringLength(20)]
        public override string LastName { get; set; }
    }
}
