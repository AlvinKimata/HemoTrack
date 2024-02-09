using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HemoTrack.Data;
using HemoTrack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using HemoTrack.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace HemoTrack.Controllers
{
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public PatientController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Get the current user ID from the user claims.
            string currentUserName = User.Identity.Name;
            var patients = await _context.User.OfType<Patient>().ToListAsync();

            var doctors = await _context.User.OfType<Doctor>().ToListAsync();
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
                PatientDashboardVM patientDashboardVM = new PatientDashboardVM
                {
                    FirstName = currentUser.FirstName + " " + currentUser.LastName,
                    Doctors = doctors,
                    Patients = patients,
                    Email = currentUser.Email,
                    UserName = currentUser.UserName,
                    Appointments = appointmentschedule
                };
                return View(patientDashboardVM);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Doctors()
        {
            if (ModelState.IsValid)
            {
                // Get the current user ID from the user claims.
                string currentUserName = User.Identity.Name;
                var patients = await _context.User.OfType<Patient>().ToListAsync();
                var doctors = await _context.User.OfType<Doctor>().ToListAsync();
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
                            Patient =  _context.User.OfType<Patient>().FirstOrDefault(),
                        });
                    }
                }

                var currentUser = _context.User.OfType<Patient>().FirstOrDefault(u => u.UserName == currentUserName);
                if (currentUser != null)
                {
                    PatientDashboardVM patientDashboardVM = new PatientDashboardVM
                    {
                        FirstName = currentUser.FirstName + " " + currentUser.LastName,
                        Doctors = doctors,
                        Patients = patients,
                        Email = currentUser.Email,
                        UserName = currentUser.UserName,
                        Appointments = appointmentschedule
                    };
                    return View(patientDashboardVM);
                }
                return NotFound();
            }
            return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ScheduleAppointment()
        {
            // Get the current user ID from the user claims.
            string currentUserName = User.Identity.Name;
            var patients = await _context.User.OfType<Patient>().ToListAsync();

            var doctors = await _context.User.OfType<Doctor>().ToListAsync();
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
                PatientDashboardVM patientDashboardVM = new PatientDashboardVM
                {
                    FirstName = currentUser.FirstName + " " + currentUser.LastName,
                    Doctors = doctors,
                    Patients = patients,
                    Email = currentUser.Email,
                    UserName = currentUser.UserName,
                    Appointments = appointmentschedule
                };
                return View(patientDashboardVM);
            }
            return NotFound();
        }

    };
}