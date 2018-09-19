using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Capstone.Web.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "A username is required")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "A password is required")]
        [MinLength(8, ErrorMessage = "Password must be 8 characters or more")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        public string LoginError { get; set; }
    }
}