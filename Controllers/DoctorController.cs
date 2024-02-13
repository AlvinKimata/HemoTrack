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
            var userId = await TempData["UserId"].ToString();
            var doctor = await _userManager.FindByIdAsync(userId);
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
            var userId = TempData["UserId"].ToString();
            TempData.Keep();
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
        public async Task<IActionResult> ListAppointments()
        {
            var doctor = GetCurrentDoctorAsync();

            if (doctor == null)
            {
                return NotFound();
            }
            var doctors = await GetAllDoctorsAsync();
            var patients = await GetAllPatientsAsync();

            var patientDashboardVM = new PatientDashboardVM
            {
                FirstName = currentUser.FirstName + " " + currentUser.LastName,
                Patients = patients,
                Email = currentUser.Email,
                UserName = currentUser.UserName,
                Doctors = doctors,
            };

            var appointmentRegisterVM = new AppointmentRegisterVM
            {
                Doctors = doctors // Initialize the Doctors property with the retrieved doctors
            };

            patientDashboardVM.Appointments = await GetAppointmentsAsync();
            patientDashboardVM.appointmentRegisterVM = appointmentRegisterVM; // Assign the appointmentRegisterVM to the appropriate property

            return View(patientDashboardVM);
        }


        [HttpGet]
        public IActionResult Patient()
        {
            var doctorDashboardVM = new DoctorDashboardVM();
            doctorDashboardVM.Patients = _context.User.OfType<Patient>().ToList();
            return View(doctorDashboardVM);
        }

        [HttpGet]
        public IActionResult Schedule()
        {
            return View();
        }

        
    }
}
