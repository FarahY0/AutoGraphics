using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace AutoGraphic.Models
{
    public class ApplicationUser:IdentityUser
    {

        //public string ConfirmPassword { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        [Display(Name = "Profile Image")]
        public string ProfileImage { get; set; }
        public string DepartmentId { get; set; }
        public virtual Department Department { get; set; }
    }
}
