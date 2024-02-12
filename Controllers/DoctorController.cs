using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HemoTrack.Data;
using HemoTrack.Models;
using HemoTrack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace HemoTrack.Controllers
{
    public class DoctorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public DoctorController(ApplicationDbContext context, UserManager<User> _userManager)
        {
            _context = context;
            _userManager = _userManager;
        }


        public async Task<IActionResult> Index(string id)
        {
            // Get the current user ID from the user claims.

            var doctor = await _userManager.FindByIdAsync(id);
            var patients = await _context.User.OfType<Patient>().ToListAsync();
            var doctors = await _context.User.OfType<Doctor>().ToListAsync();
            var appointments = await _context.Appointment.ToListAsync();

            if (doctor != null)
            {
                DoctorDashboardVM doctorDashboardVM = new DoctorDashboardVM
                {
                    DoctorId = Convert.ToInt32(doctor.Id),
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