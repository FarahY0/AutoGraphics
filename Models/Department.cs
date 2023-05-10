using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoGraphic.Data;
using AutoGraphic.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace AutoGraphic.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="This feild is frequired")]
        [Display(Name ="Department Name")]
        
        public string Name { get; set; }
        public int Count { get; set; }
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
        //public RegisterViewModel RegisterViewModel { get; set; }

    }
}
