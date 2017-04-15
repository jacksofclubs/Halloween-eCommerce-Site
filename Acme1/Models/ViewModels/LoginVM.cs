using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Acme1.Models.ViewModels
{
    public class LoginVM
    {
        [Required, MaxLength(50)]
        public string Username { get; set; }
        [Required, MaxLength(20)]
        public string Password { get; set; }
    }
}