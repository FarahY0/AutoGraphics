using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AutoGraphic.Models.ViewModels
{
    public class EditProfileViewModel : ApplicationUser
    {
        public int EditProfileId { get; set; }
        public IFormFile ProfileImage { get; set; }
    }
}
