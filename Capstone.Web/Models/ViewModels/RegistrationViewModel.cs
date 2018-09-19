using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.ComponentModel.DataAnnotations;

namespace Capstone.Web.Models.ViewModels
{
    public class RegistrationViewModel
    { 

        [Required(ErrorMessage = "first name required")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "last name required")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "email is required")]
        [EmailAddress(ErrorMessage = "enter a valid email address.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "username required")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "password required")]
        [MinLength(8, ErrorMessage = "password must be 8 characters or more.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "please confirm password")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string RegistrationError { get; set; }
    }
}