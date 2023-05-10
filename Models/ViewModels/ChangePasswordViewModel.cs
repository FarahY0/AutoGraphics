using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoGraphic.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please write your password")]
        [DataType(DataType.Password, ErrorMessage = "example : Farah@34")]
        [Display(Name = "Current Password")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Please write your password")]
        [DataType(DataType.Password, ErrorMessage = "example : Farah@34")]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Please confirm your password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password does Not Match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

    }
}
