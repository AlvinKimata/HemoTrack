using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HemoTrack.Data;
using HemoTrack.Models;
using HemoTrack.ViewModels;

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

        private async Task<User> GetCurrentPatientAsync()
        {
            string currentUserName = User.Identity.Name;
            return await _context.User.OfType<Patient>().FirstOrDefaultAsync(u => u.UserName == currentUserName);
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
            var currentUser = await GetCurrentPatientAsync();
            if (currentUser == null)
            {
                return NotFound();
            }

            var doctors = await GetAllDoctorsAsync();
            var patients = await GetAllPatientsAsync();
            var appointments = await _context.Appointment
                                            .Where(appointment => appointment.Patient.Id == currentUser.Id)
                                            .Distinct()
                                            .ToListAsync();

            // Your appointment scheduling logic can be moved here

            var patientDashboardVM = new PatientDashboardVM
            {
                FirstName = currentUser.FirstName + " " + currentUser.LastName,
                Doctors = doctors,
                Patients = patients,
                Email = currentUser.Email,
                UserName = currentUser.UserName,
                Appointments = appointments
            };

            return View(patientDashboardVM);
        }

        [HttpGet]
        public async Task<IActionResult> Doctors()
        {
            var currentUser = await GetCurrentPatientAsync();
            if (currentUser == null)
            {
                return NotFound();
            }

            var doctors = await GetAllDoctorsAsync();
            var patients = await GetAllPatientsAsync();
            
            // Your appointment scheduling logic can be moved here
            var appointments =  await _context.Appointment
                                            .Where(appointment => appointment.Patient.Id == currentUser.Id)
                                            .Distinct()
                                            .ToListAsync();

           

            var patientDashboardVM = new PatientDashboardVM
            {
                FirstName = currentUser.FirstName + " " + currentUser.LastName,
                Doctors = doctors,
                Patients = patients,
                Email = currentUser.Email,
                UserName = currentUser.UserName,
                Appointments = appointments
            };

            return View(patientDashboardVM);
        }

        [HttpGet]
        public async Task<IActionResult> ScheduleAppointment()
        {
            var currentUser = await GetCurrentPatientAsync();
            if (currentUser == null)
            {
                return NotFound();
            }

            var doctors = await GetAllDoctorsAsync();
            var patients = await GetAllPatientsAsync();
            var appointments = await GetAppointmentsAsync();

            // Your appointment scheduling logic can be moved here

            var patientDashboardVM = new PatientDashboardVM
            {
                FirstName = currentUser.FirstName + " " + currentUser.LastName,
                Doctors = doctors,
                Patients = patients,
                Email = currentUser.Email,
                UserName = currentUser.UserName,
                Appointments = appointments
            };

            return View(patientDashboardVM);
        }

        [HttpGet]
        public async Task<IActionResult> ListAppointments()
        {
            var currentUser = await GetCurrentPatientAsync();
            if (currentUser == null)
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

            patientDashboardVM.Appointments = await _context.Appointment
                                            .Where(appointment => appointment.Patient.Id == currentUser.Id)
                                            .Distinct()
                                            .ToListAsync();
            patientDashboardVM.appointmentRegisterVM = appointmentRegisterVM; // Assign the appointmentRegisterVM to the appropriate property

            return View(patientDashboardVM);
        }

        [HttpPost]
        public async Task<IActionResult> ListAppointments(AppointmentRegisterVM model)
        {
            var currentUserName = await GetCurrentPatientAsync();
            var currentUser = _context.User.OfType<Patient>().FirstOrDefault(u => u.UserName == currentUserName.UserName);
            model.Doctors = await GetAllDoctorsAsync();

            if (!ModelState.IsValid)
            {
                var doctorName = model.Doctor; //Get the name of the doctor
                var doctor = await _context.User.OfType<Doctor>().FirstOrDefaultAsync(m => m.FirstName == doctorName.FirstName);

                var appointmentRegisterVM = new Appointment
                {
                    Title = model.Title,
                    AppointmentDate = model.AppointmentDate,
                    AppointmentTime = model.AppointmentTime,
                    Patient = currentUser,
                    Doctor = doctor,
                };

                var result = _context.Appointment.Add(appointmentRegisterVM);

                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"Unable to add appointment. Error details : {ex.Message}");
                    throw;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Unexpected error occurred while adding appointment - Error details: {ex.Message}");
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task <IActionResult> Settings()
        {
            string currentUserName = User.Identity.Name;
            var patient = await _userManager.FindByNameAsync(currentUserName);

            var patientDashboardVM = new PatientDashboardVM();
            doctorDashboardVM.Doctor = await _context.User.OfType<Doctor>().FirstOrDefaultAsync(m => m.Email == doctor.Email);
        

            return View(doctorDashboardVM);
        }
        
        [HttpPost]
        public async Task<IActionResult> Settings(Patient model, string action)
        {
            if (ModelState.IsValid)
            {
                if (action == "modify")
                {
                    //Modify doctor's details.
                    string currentUserName = User.Identity.Name;
                    var doctor = await _context.User.OfType<Doctor>().FirstOrDefaultAsync(m => m.Email == model.Email);
                    if (doctor != null)
                    {
                        doctor.FirstName = model.FirstName;
                        doctor.LastName = model.LastName;
                        doctor.Email = model.Email;
                        doctor.Nic = model.Nic;
                        doctor.PhoneNumber = model.PhoneNumber;
                        doctor.Speciality = model.Speciality;

                        _context.Update(doctor);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(model);

        }
    }
}