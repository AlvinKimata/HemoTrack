using HemoTrack.Models;
using HemoTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HemoTrack.Controllers
{
    public class BaseController : Controller
    {
        protected readonly UserManager<User> _userManager;
        protected readonly SignInManager<User> _signInManager;
        protected readonly RoleManager<IdentityRole> _roleManager;

        public BaseController(UserManager<User> userManager, 
            SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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
                    var user = await _userManager.FindByEmailAsync(model.Email);

                    //Check user's role and redirect accordingly.
                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        return RedirectToAction("Index", "Administrator");
                    }

                    else if (await _userManager.IsInRoleAsync(user, "Doctor"))
                    {
                        return RedirectToAction("Index", "Doctor");
                    }

                    else if (await _userManager.IsInRoleAsync(user, "Patient"))
                    {
                        return RedirectToAction("Index", "Patient");
                    }
                    return RedirectToAction("Index", "Home");

                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt");
            }

            return View(model);
        }
    }
}