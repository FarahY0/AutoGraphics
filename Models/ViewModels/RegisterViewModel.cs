using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoGraphic.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Your Name")]
        [DataType(DataType.Text, ErrorMessage = "Please Write Your Name")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
       
        [Required(ErrorMessage ="Please write your email")]
        [DataType(DataType.EmailAddress,ErrorMessage ="example : farah@gmail.com")]
        [MaxLength(100,ErrorMessage = "The number of characters must be less than 100")]
        [Display(Name ="Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please write your password")]
        [DataType(DataType.Password, ErrorMessage = "example : Farah@34")]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Password does Not Match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage ="Please write your phone number")]
        public string Mobile { get; set; }
    

        
    }
}
