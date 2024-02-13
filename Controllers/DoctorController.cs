using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HemoTrack.Data;
using HemoTrack.Controllers;
using HemoTrack.Models;
using HemoTrack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace HemoTrack.Controllers
{
    public class DoctorController : BaseController
    {
        private readonly ApplicationDbContext _context;
        public string IndexModel = "Doctor";
        public DoctorController(ApplicationDbContext context,
                            UserManager<User> userManager,
                            SignInManager<User> signInManager,
                            RoleManager<IdentityRole> roleManager)
                            : base(userManager, signInManager, roleManager)
        {
            _context = context;
        }


        private async Task<User> GetCurrentDoctorAsync()
        {
            string doctorName = User.Identity.Name;
            return await _context.User.OfType<Doctor>().FirstOrDefaultAsync(m => m.FirstName == doctorName);
        }

        private async Task<List<Doctor>> GetAllDoctorsAsync()
        {
            return await _context.User.OfType<Doctor>().ToListAsync();
        }

        private async Task<List<Patient>> GetAllPatientsAsync()
        {
            return await _context.User.OfType<Patient>().ToListAsync();
        }

        private async Task<List<Appointment>> GetAppointmentsAsync()
        {
            return await _context.Appointment.ToListAsync();
        }

        // [HttpPost]
        // public async Task<IActionResult> Login(LoginViewModel model, string IndexModel)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
        //         if (result.Succeeded)
        //         {
        //             var user = await _userManager.FindByEmailAsync(model.Email);
        //             return RedirectToAction("Index", "Doctor"); // Redirect to a success action
        //         }
        //     }

        //     return View(model);
        // }

        [HttpGet]
        public async Task<IActionResult> Index()
        {   
            var userId = TempData["UserId"].ToString();
            var doctor = await _userManager.FindByIdAsync(userId);
            
            var doctors = await GetAllDoctorsAsync();
            var patients = await GetAllPatientsAsync();
            var appointments = await GetAppointmentsAsync();

            if (doctor != null)
            {
                DoctorDashboardVM doctorDashboardVM = new DoctorDashboardVM
                {
                    FirstName = doctor.FirstName + " " + doctor.LastName,
                    Email = doctor.Email,
                    Doctors = doctors,
                    Patients = patients,
                    Appointments = appointments
                };
                return View(doctorDashboardVM);
            }
            return NotFound();
            
        }

        [HttpGet]
        public IActionResult Appointment()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Patient()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Schedule()
        {
            return View();
        }

        
    }
}
