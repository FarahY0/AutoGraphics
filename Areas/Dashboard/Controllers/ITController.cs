using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoGraphic.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoGraphic.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]


    public class ITController : Controller
    {
        private readonly AppDbContext _context;

        public ITController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var departments = _context.Departments.Include(d => d.ApplicationUsers).ToList();
            return View(departments);
        }
    }
}
