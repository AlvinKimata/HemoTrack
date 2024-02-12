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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var doctor = await GetCurrentDoctorAsync();
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