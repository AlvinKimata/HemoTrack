using HemoTrack.Models;
using HemoTrack.Data;
using HemoTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace HemoTrack.Controllers
{
    public class AccountController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<User> userManager, ApplicationDbContext context,
            SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager): base(userManager, signInManager, roleManager)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = _userManager.Users;
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use.");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Patient
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth,
                    PhoneNumber = model.PhoneNumber,
                    Nic = model.Nic,
                    Address = model.Address,
                    Password = model.Password
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                //Add the patient role to a patient once registered.
                var roleResult = await _userManager.AddToRoleAsync(user, "Patient");

                if (result.Succeeded && roleResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Patient");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }


        //Login actions.
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (!result.Succeeded)
                {
                    // Get the user
                    var user = await _userManager.FindByEmailAsync(model.Email);

                    //By default set the user's role to a patient.

                    if (user != null)
                    {
                        // Check user's role and redirect accordingly
                        if (await _userManager.IsInRoleAsync(user, "Admin"))
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return RedirectToAction("Index", "Administrator");
                        }
                        else if (await _userManager.IsInRoleAsync(user, "Doctor"))
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return RedirectToAction("Index", "Doctor");
                        }
                        else if (await _userManager.IsInRoleAsync(user, "Patient"))
                        {   
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return RedirectToAction("Index", "Patient");
                        }
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    // If the user's role doesn't match any expected roles, handle accordingly
                    return RedirectToAction("Index", "Home");

                    }
                    // If the user does not exist.
                    ModelState.AddModelError("", "User not registered yet.");

                    // return RedirectToAction("Index", "Home");
                }

            }

            return View(model);
        }

    }
}