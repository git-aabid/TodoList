using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TODO.Models
{
    public class UserModel
    {
        [Required]
        [DisplayName("Username")]
        public string UserName { get; set; }
        [Required]
        [DisplayName("Password")]
        public string Password { get; set; }
        public int UserId { get; set; }
    }
}
