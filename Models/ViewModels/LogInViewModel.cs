using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoGraphic.Models.ViewModels
{
    public class LogInViewModel
    {
        [Required(ErrorMessage = "User Name OR Email")]
        [DataType(DataType.Text, ErrorMessage = "Please Write Your Name or Email")]
        [Display(Name = "User Name OR Email")]
        public string UsernameOrEmail { get; set; }
    
        [Required(ErrorMessage = "Please write your password")]
        [DataType(DataType.Password, ErrorMessage = "example : Farah@34")]
        [Display(Name = "Password")]
        public string Password { get; set; }
    
        public bool RememberMe { get; set; }
    }
}
