using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoGraphic.Data;
using AutoGraphic.Models;
using AutoGraphic.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AutoGraphic.Areas.Dashboard.Controllers
{
   

    [Area("Dashboard")]
   
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public UsersController(UserManager<ApplicationUser> userManager , AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        private bool UserExists(string id)
        {
            return _userManager.FindByIdAsync(id) != null;
        }

        //public IActionResult Charts()
        //{
        //    var users = _context.ApplicationUser.ToList();
        //    var userCount = users.Count(u => u.Role == 2);
        //    var adminCount = users.Count(u => u.Role == 1);

        //    var model = new UserAdminCountModel
        //    {
        //        UserCount = userCount,
        //        AdminCount = adminCount
        //    };
        //    return View(model);
        //}
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            _context.Update(user);
            await _context.SaveChangesAsync();

            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var departments = _context.Departments.ToList();
            var departmentSelectList = new SelectList(departments, "Id", "Name", user.DepartmentId);

            ViewData["DepartmentSelectList"] = departmentSelectList;

            var selectedDepartment = departments.FirstOrDefault(d => d.Id.ToString() == user.DepartmentId);

            ViewData["DepartmentName"] = selectedDepartment?.Name;

            //_context.Update(user);
            //await _context.SaveChangesAsync();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Email,UserName,Password,PhoneNumber,DepartmentId,Department.Name", "DepartmentId1")] ApplicationUser user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var currentUser = await _userManager.FindByIdAsync(id);
                    currentUser.Email = user.Email;
                    currentUser.UserName = user.UserName;
                    currentUser.PhoneNumber = user.PhoneNumber;
                    currentUser.Password = user.Password;
                    currentUser.DepartmentId = user.DepartmentId;
                    //currentUser.Name = user.Department.Name;
                    await _userManager.UpdateAsync(currentUser);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        [HttpGet]
        public ActionResult GetAllDepartment()
        {
            List<Department> departments = _context.Departments.ToList();

            return Json(departments);
        }
        // GET: Dashboard/Departments/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.Users.Include(u => u.Department)
                                                 .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            await _context.Entry(user).Reference(u => u.Department).LoadAsync(); // Load the Department navigation property
            var departments = _context.Departments.ToList();
            var selectedDepartment = departments.FirstOrDefault(d => d.Id.ToString() == user.DepartmentId);

            ViewData["DepartmentName"] = selectedDepartment?.Name;

            _context.Update(user);
            await _context.SaveChangesAsync();

            return View(user);
        }

    }
}
