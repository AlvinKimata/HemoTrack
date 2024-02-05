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
        private readonly UserManager<User> userManager;

        public DoctorController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            userManager = userManager;
        }


        public async Task<IActionResult> Index(string id)
        {
            // Get the current user ID from the user claims.

            var doctor = await userManager.FindByIdAsync(id);
            var patients = await _context.User.OfType<Patient>().ToListAsync();
            var doctors = await _context.Doctor.ToListAsync();
            var appointmentschedule = await _context.Appointment.ToListAsync();
            var schedule = await _context.Schedule.ToListAsync();
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
                        Patients = new List<Patient> { _context.User.OfType<Patient>().FirstOrDefault() },
                    });
                }
            }

            if (doctor != null)
            {
                DoctorDashboardVM doctorDashboardVM = new DoctorDashboardVM
                {
                    DoctorId = Convert.ToInt32(doctor.Id),
                    FirstName = doctor.FirstName + " " + doctor.LastName,
                    Email = doctor.Email,
                    Doctors = doctors,
                    Patients = patients,
                    Appointments = appointments,
                    Schedules = schedule
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

        // [HttpPost]
        // public IActionResult Appointment()
        // {
        //     return View();
        // }

        [HttpGet]
        public IActionResult Patient()
        {
            return View();
        }

        // [HttpPost]
        // public IActionResult Patient()
        // {
        //     return View();
        // }

        [HttpGet]
        public IActionResult Schedule()
        {
            return View();
        }

        // [HttpPost]
        // public IActionResult Schedule()
        // {
        //     return View();
        // }
        
    }
}