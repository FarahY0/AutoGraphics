using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoGraphic.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;



using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MailKit.Security;
using System.Net;
using AutoGraphic.Controllers;
using AutoGraphic.Models;
using MailKit.Net.Smtp;
using System.Security.Claims;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoGraphic.Data;

namespace AutoGraphic.Controllers
{

}
    public class AccountController : Controller
    {

        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly AppDbContext _context;



    public AccountController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager , RoleManager<IdentityRole> roleManager , AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        _roleManager = roleManager;
        _context = context;
            //_emailSender = emailSender;
        }
    [HttpGet]
    public IActionResult LogIn()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> LogIn(LogInViewModel model, string ReturnUrl)
    {
        ApplicationUser user;
        if (model.UsernameOrEmail.Contains("@"))
        {
            user = await _userManager.FindByEmailAsync(model.UsernameOrEmail);
        }
        else
        {
            user = await _userManager.FindByNameAsync(model.UsernameOrEmail);
        }
        if (user == null)
        {
            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }
        var result = await
        _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }
        if (ReturnUrl != null) return LocalRedirect(ReturnUrl);
        return RedirectToAction("Index", "Home", new
        {
            area = "Dashboard"
        });
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();

    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View();
        var newUser = new ApplicationUser
        {
            Email = model.Email,
            UserName = model.UserName
        };
        IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);
        if (!result.Succeeded)
        {
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
        }
        return RedirectToAction("LogIn");
    }
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(LogIn));
    }
    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Debug.WriteLine($"User ID: {userId}");
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        await _signInManager.RefreshSignInAsync(user);
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }


    [HttpGet]
    public async Task<IActionResult> Profile(string id)
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
    public async Task<IActionResult> Profile(string id, [Bind("Id,Email,UserName,Password,PhoneNumber,DepartmentId,Department.Name", "DepartmentId1")] ApplicationUser user)
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

    //[HttpGet]
    //public IActionResult EditProfile()
    //{

    //    return View();
    //}

    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> EditProfile(string id, [Bind("Id,Email,UserName,PhoneNumber")] ApplicationUser user)
    //{
    //    if (id != user.Id)
    //    {
    //        return NotFound();
    //    }

    //    if (ModelState.IsValid)
    //    {
    //        try
    //        {
    //            var currentUser = await _userManager.FindByIdAsync(id);
    //            currentUser.Email = user.Email;
    //            currentUser.UserName = user.UserName;
    //            currentUser.PhoneNumber = user.PhoneNumber;

    //            await _userManager.UpdateAsync(currentUser);
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            if (!UserExists(user.Id))
    //            {
    //                return NotFound();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }
    //        return RedirectToAction(nameof(Index));
    //    }
    //    return View(user);
    //}
    private bool UserExists(string id)
    {
    throw new NotImplementedException();
}


}


// [HttpGet]
//public IActionResult ForgotPassword()
// {
//     return View();
// }
// [HttpPost]
// [AllowAnonymous]
// [ValidateAntiForgeryToken]
// public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
// {
//     if (ModelState.IsValid)
//     {
//         var user = await _userManager.FindByNameAsync(model.UserName);
//         if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
//         {
//             // Don't reveal that the user does not exist or is not confirmed
//             return RedirectToAction(nameof(ForgotPasswordConfirmed));
//         }

//         // Generate a random password
//         var newPassword = GenerateRandomPassword();

//         // Reset the user's password
//         var result = await _userManager.ResetPasswordAsync(user, await _userManager.GeneratePasswordResetTokenAsync(user), newPassword);

//         if (result.Succeeded)
//         {
//             // Send the password reset email
//             var emailSender = new EmailSender("f.y.abusiam@gmail.com", "Farah-AbuSiam-2000");
//             var emailSubject = "Password Reset Request";
//             var emailMessage = $"Your new password is: {newPassword}. Please log in using your new password and change it as soon as possible.";
//             await emailSender.SendEmailAsync(model.Email, emailSubject, emailMessage);

//             return RedirectToAction(nameof(ForgotPasswordConfirmed));
//         }

//         foreach (var error in result.Errors)
//         {
//             ModelState.AddModelError(string.Empty, error.Description);
//         }
//     }

//     // If we got this far, something failed, redisplay the form
//     return View(model);
// }

// private string GenerateRandomPassword()
// {
//     // Generate a random password using the built-in .NET Random class
//     const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
//     var random = new Random();
//     var password = new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
//     return password;
// }

// [AllowAnonymous]
// public ActionResult ForgotPasswordConfirmed()
// {
//     return View();
// }






//[HttpPost]
//[AllowAnonymous]
//[ValidateAntiForgeryToken]
//public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
//{

//    returnUrl = returnUrl ?? Url.Content("~/");
//    if (ModelState.IsValid)
//    {
//        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
//        var result = await _userManager.CreateAsync(user, model.Password);
//        if (result.Succeeded)
//        {
//            var userRole = new IdentityRole("User");
//            await _roleManager.CreateAsync(userRole);
//            // Add user role
//            await _userManager.AddClaimAsync(user, new Claim("Departments", model.Departments)); // Add department claim

//            // Sign in the user
//            await _signInManager.SignInAsync(user, isPersistent: false);

//            // Redirect to department dashboard
//            switch (model.Departments)
//            {
//                case "IT":
//                    return RedirectToAction("Index", "IT", new { area = "Dashboard" });
//                case "Marketing":
//                    return RedirectToAction("Index", "Marketing", new { area = "Dashboard" });


//                // Add more cases for each department
//                default:
//                    return RedirectToAction("Index", "Home", new { area = "Dashboard" });
//            }
//        }
//        foreach (var error in result.Errors)
//        {
//            ModelState.AddModelError(string.Empty, error.Description);
//        }
//    }

//    // If we got this far, something failed, redisplay form
//    return View(model);
//}


//[HttpGet]

//    public IActionResult LogIn()
//        {
//            return View();
//        }
//    public async Task<IActionResult> LogIn(LogInViewModel model, string ReturnUrl)
//    {
//        ApplicationUser user;
//        if (model.UsernameOrEmail.Contains("@"))
//        {
//            user = await _userManager.FindByEmailAsync(model.UsernameOrEmail);
//        }
//        else
//        {
//            user = await _userManager.FindByNameAsync(model.UsernameOrEmail);
//        }
//        if (user == null)
//        {
//            ModelState.AddModelError("", "Login ve ya parol yalnisdir");
//            return View(model);
//        }
//        var result = await
//        _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
//        if (!result.Succeeded)
//        {
//            ModelState.AddModelError("", "Invalid login attempt.");
//            return View(model);
//        }
//        if (ReturnUrl != null) return LocalRedirect(ReturnUrl);
//        return RedirectToAction("Index", "Home", new
//        {
//            area = "Dashboard"
//        });
//    }
//[HttpPost]
//[AllowAnonymous]
//[ValidateAntiForgeryToken]
//public async Task<IActionResult> LogIn(LogInViewModel model, string returnUrl = null)
//{
//    returnUrl ??= Url.Content("~/");
//    if (ModelState.IsValid)
//    {
//        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
//        if (result.Succeeded)
//        {
//            var user = await _userManager.FindByEmailAsync(model.Email);
//            if (await _userManager.IsInRoleAsync(user, "Admin"))
//            {
//                return LocalRedirect(returnUrl);
//            }
//            else
//            {
//                await _signInManager.SignOutAsync();
//                return RedirectToAction("Index", "Home");
//            }
//        }
//        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
//        return View(model);
//    }
//    // If we got this far, something failed, redisplay form
//    return View(model);
//}


//public IActionResult ChangePassword()
//    {
//        return View();
//    }


//[HttpPost]
//[ValidateAntiForgeryToken]
//public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
//{
//    if (!ModelState.IsValid)
//    {
//        return View(model);
//    }
//var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//Debug.WriteLine($"User ID: {userId}");
//var user = await _userManager.GetUserAsync(User);
//    if (user == null)
//    {
//        return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
//    }

//    var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
//    if (!result.Succeeded)
//    {
//        foreach (var error in result.Errors)
//        {
//            ModelState.AddModelError(string.Empty, error.Description);
//        }
//        return View(model);
//    }

//    await _signInManager.RefreshSignInAsync(user);
//    return RedirectToAction(nameof(HomeController.Index), "Home");
//}

//[HttpPost]
//[ValidateAntiForgeryToken]
//public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
//{
//    if (!ModelState.IsValid)
//    {
//        return View(model);
//    }

//    var user = await _userManager.GetUserAsync(User);

//    if (user == null)
//    {
//        return NotFound();
//    }

//    var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

//    if (!changePasswordResult.Succeeded)
//    {
//        foreach (var error in changePasswordResult.Errors)
//        {
//            ModelState.AddModelError(string.Empty, error.Description);
//        }

//        return View(model);
//    }

//    await _signInManager.RefreshSignInAsync(user);

//    return RedirectToAction(nameof(LogIn));
//}







//public IActionResult Logut()
//{
//    HttpContext.Session.Clear();
//    return RedirectToAction("LogIn");
//}


//[HttpPost]
//[ValidateAntiForgeryToken]
//public async Task<IActionResult> EditProfile(string id, [Bind("Id,Email,UserName,PhoneNumber")] EditProfileViewModel user)
//{
//    if (id != user.Id)
//    {
//        return NotFound();
//    }

//    if (ModelState.IsValid)
//    {
//        try
//        {
//            var currentUser = await _userManager.FindByIdAsync(id);
//            currentUser.Email = user.Email;
//            currentUser.UserName = user.UserName;
//            currentUser.PhoneNumber = user.PhoneNumber;

//            await _userManager.UpdateAsync(currentUser);
//        }
//        catch (DbUpdateConcurrencyException)
//        {
//            if (!UserExists(user.Id))
//            {
//                return NotFound();
//            }
//            else
//            {
//                throw;
//            }
//        }
//        return RedirectToAction(nameof(Index));
//    }
//    return View(user);
//}




//[HttpPost]
//public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
//{
//    if (ModelState.IsValid)
//    {
//        // Generate a random password
//        var random = new Random();
//        var password = new string(Enumerable.Repeat("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
//            .Select(s => s[random.Next(s.Length)]).ToArray());

//        // Update the user's password in the database
//        var user = await _userManager.FindByNameAsync(model.UserName);
//        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
//        {
//            // Don't reveal that the user does not exist or is not confirmed
//            return RedirectToAction("ForgotPasswordConfirmation");
//        }

//        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
//        var result = await _userManager.ResetPasswordAsync(user, token, password);

//        if (result.Succeeded)
//        {
//            // Send the new password to the user by email
//            var emailSubject = "Your new password";
//            var emailMessage = $"Your new password is: {password}";
//            await _emailSender.SendEmailAsync(model.Email, emailSubject, emailMessage);

//            return RedirectToAction("ForgotPasswordConfirmed");
//        }

//        foreach (var error in result.Errors)
//        {
//            ModelState.AddModelError(string.Empty, error.Description);
//        }
//    }

//    // If we got this far, something failed, redisplay form
//    return View(model);
//}









//}

