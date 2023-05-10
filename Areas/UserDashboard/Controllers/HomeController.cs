using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AutoGraphic.Areas.UserDashboard.Controllers
{
    public class HomeController : Controller
    {
        [Area("UserDashboard")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
