using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

using HemoTrack.Data;
using HemoTrack.Models;
using HemoTrack.ViewModels;

namespace HemoTrack.Controllers
{
    public class AdministratorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AdministratorController(ApplicationDbContext context, 
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {   // Get the current user ID from the user claims.
            string currentUserName = User.Identity.Name;
            var patients = await _context.User.OfType<Patient>().ToListAsync();
            var doctors = await _context.Doctor.ToListAsync();
            var appointmentschedule = await _context.Appointment.ToListAsync();
            var today = DateTime.Today;
            var currentTime = DateTime.Now;

            var endOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
            var appointments = new List<Appointment>();
            for (var date = today; date <= endOfMonth; date = date.AddDays(1))
            {
                var dayOfWeek = date.DayOfWeek;
                if (dayOfWeek != DayOfWeek.Saturday && dayOfWeek != DayOfWeek.Sunday)
                {
                    appointments.Add(new Appointment
                    {
                        AppointmentDate = date,
                        Title = $"Appointment on {date.ToShortDateString()}",
                        Patient = _context.User.OfType<Patient>().FirstOrDefault(),
                    });
                }
            }

            var currentUser = _context.User.OfType<Patient>().FirstOrDefault(u => u.UserName == currentUserName);
            if (currentUser != null)
            {
                AdministratorDashboardVM administratorDashboardVM = new AdministratorDashboardVM
                {
                    Doctors = doctors,
                    Patients = patients,
                    Email = currentUser.Email,
                    Appointments = appointmentschedule
                };
                return View(administratorDashboardVM);
            }
            return NotFound();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        

        [HttpPost]
        public async Task<IActionResult> Register(AdminRegisterVM model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Administrator.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email is already registered");
                    return View(model);
                }

                //Create a new admin.
                var user = new Administrator
                {
                    Email = model.Email,
                    Password = model.Password
                };
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
        
            }
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, 
                                                                        model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Administrator");

                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Doctors()
        {
            var doctorDashboardVM = new DoctorDashboardVM();
            doctorDashboardVM.Doctors =  _context.User.OfType<Doctor>().ToList();
            return View(doctorDashboardVM);
        }

        [HttpGet]
        public IActionResult Patient()
        {
            var administratorDashboardVM = new AdministratorDashboardVM();
            administratorDashboardVM.Patients = _context.User.OfType<Patient>().ToList();
            return View(administratorDashboardVM);
        }


        [HttpGet]
        public IActionResult Appointment()
        {
            var administratorDashboardVM = new AdministratorDashboardVM();
            administratorDashboardVM.Appointments = _context.Appointment.ToList();
            return View(administratorDashboardVM);
        }

        [HttpGet]
        public IActionResult Settings()
        {
            return View();
        }


        // [HttpGet]
        // public IActionResult Schedule()
        // {
        //     var administratorDashboardVM = new AdministratorDashboardVM();
        //     administratorDashboardVM.Schedules = _context.Schedule.ToList();
        //     return View(administratorDashboardVM);
        // }


        [HttpPost]
        public async Task<IActionResult> Doctors(DoctorRegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var existingDoctor = await _userManager.FindByEmailAsync(model.Email);
                if (existingDoctor != null)
                {
                    ModelState.AddModelError("Doctor", "Doctor is already registered");
                    return View(model);
                }
                //Register a new doctor
                var doctorRegisterVM = new Doctor{
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.FirstName,
                    Email = model.Email,
                    Nic = model.Nic,
                    PhoneNumber = model.PhoneNumber,
                    Speciality = model.Speciality,
                    Password = model.Password,
                };

                var result =  await _userManager.CreateAsync(doctorRegisterVM, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Doctors");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return RedirectToAction("Doctors");
            }


            return View(model);
        }

        [HttpGet]
        public IActionResult EditDoctor()
        {
            return View();
        }

        // [HttpPost]
        // public IActionResult EditDoctor()
        // {
        //     return View();
        // }

        [HttpGet]
        public IActionResult DeleteDoctor()
        {
            return View();
        }

        // [HttpPost]
        // public IActionResult DeleteDoctor()
        // {
        //     return View();
        // }

    }
}