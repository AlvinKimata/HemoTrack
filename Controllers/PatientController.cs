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

        public async Task<IActionResult> Index()
        {
            // Get the current user ID from the user claims.
            string currentUserName = User.Identity.Name;
            var patients = await _context.Patient.ToListAsync();
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
                        Patients = new List<Patient> { _context.Patient.FirstOrDefault() },
                    });
                }
            }

            var currentUser = _context.Patient.FirstOrDefault(u => u.UserName == currentUserName);
            if (currentUser != null)
            {
                PatientDashboardVM patientDashboardVM = new PatientDashboardVM
                {
                    FirstName = currentUser.FirstName + " " + currentUser.LastName,
                    Doctors = doctors,
                    Patients = patients,
                    Email = currentUser.Email,
                    UserName = currentUser.UserName,
                    Appointments = appointmentschedule,
                    Schedules = schedule
                };
                return View(patientDashboardVM);
            }
            return NotFound();
        }
        
        public async Task<IActionResult> Doctors()
        {
            // Get the current user ID from the user claims.
            string currentUserName = User.Identity.Name;
            var patients = await _context.Patient.ToListAsync();
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
                        Patients = new List<Patient> { _context.Patient.FirstOrDefault() },
                    });
                }
            }

            var currentUser = _context.Patient.FirstOrDefault(u => u.UserName == currentUserName);
            if (currentUser != null)
            {
                PatientDashboardVM patientDashboardVM = new PatientDashboardVM
                {
                    FirstName = currentUser.FirstName + " " + currentUser.LastName,
                    Doctors = doctors,
                    Patients = patients,
                    Email = currentUser.Email,
                    UserName = currentUser.UserName,
                    Appointments = appointmentschedule,
                    Schedules = schedule
                };
                return View(patientDashboardVM);
            }
            return NotFound();
        }
    };
}