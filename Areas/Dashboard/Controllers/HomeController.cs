using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoGraphic.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(Policy = "UserAndAdmin")]
    public class HomeController : Controller
    {
      
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
    }
}
