using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoGraphic.Areas.Dashboard.Controllers
{
    [Authorize(Policy = "UserAndAdmin")]
    [Area("Dashboard")]
    public class AccountingAndFinanceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
