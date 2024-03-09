using System;
using System.Linq;
using System.Data.Common;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

using HemoTrack.Data;
using HemoTrack.Models;
using HemoTrack.ViewModels;

namespace HemoTrack.Controllers
{   
    // [Authorize(Roles = "Admin")]
    public class AdministratorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdministratorController(ApplicationDbContext context, 
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {   // Get the current user ID from the user claims.
            string currentUserName = User.Identity.Name;
            // var currentUser = _context.User.OfType<Patient>().FirstOrDefault(u => u.UserName == currentUserName);
            var currentUser = await _userManager.FindByNameAsync(currentUserName);
            var patients = await _context.User.OfType<Patient>().ToListAsync();
            var doctors = await _context.User.OfType<Doctor>().ToListAsync();
            var appointments = await _context.Appointment.Include(m => m.Doctor)
                                                         .Include(m => m.Patient)
                                                         .ToListAsync();

            
            if (currentUser != null)
            {
                AdministratorDashboardVM administratorDashboardVM = new AdministratorDashboardVM
                {
                    Doctors = doctors,
                    Patients = patients,
                    Email = currentUser.Email,
                    Appointments = appointments
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

        private async Task<User> GetCurrentPatientAsync()
        {
            string currentUserName = User.Identity.Name;
            return await _context.User.OfType<Patient>().FirstOrDefaultAsync(u => u.UserName == currentUserName);
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
                    Password = model.Password,
                    UserName = "admin",
                    FirstName = "admin",
                    LastName = " ",
                    PhoneNumber = "0000",
                    Nic = "0000"
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                //Add admin to role.
                //Check if roles exist. Else create role for admin.

                //Add admin role by default if no role exists.
                IdentityRole identityRole = new IdentityRole
                {
                    Name = "Admin"
                };
                IdentityResult roleResult = await _roleManager.CreateAsync(identityRole);

                var roleAddResult = await _userManager.AddToRoleAsync(user, "Admin");

                if (result.Succeeded && roleAddResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Administrator");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                foreach (var error in roleAddResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
        
            }
            return View(model);
        }
        
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, 
                                                                        model.RememberMe, false);

                if (!result.Succeeded)
                {
                    return RedirectToAction("Index", "Administrator");

                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleVM model)
        {
            if(ModelState.IsValid)
            {
                //Specify unique role name to create a new role.
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                //Save the role in underlying AspNetRoles table.
                IdentityResult result = await _roleManager.CreateAsync(identityRole);

                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Administrator");
                }
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ListRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var roleDashboardVM = new RoleDashboardVM 
            {
                Roles = roles,
                UsersRoleViewModels = new List<UsersRoleViewModel>()
            };

            foreach(var role in roles)
            {
                var usersInRole = _userManager.Users;
                var usersRoleViewModel = new UsersRoleViewModel
                {
                    Role = role,
                    UsersInRole = new List<UserVM>()
                };

                foreach (var user in usersInRole)
                {
                    var userVM = new UserVM
                    {
                        user = user,
                        UserName = user.UserName
                    };

                    if (await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        userVM.IsSelected = true;
                    }

                    else
                    {
                        userVM.IsSelected = false;
                    }
                    
                    usersRoleViewModel.UsersInRole.Add(userVM);
                }
                roleDashboardVM.UsersRoleViewModels.Add(usersRoleViewModel);
            }

            return View(roleDashboardVM);
        }

        [HttpPost]
        public async Task<IActionResult> ListRoles(UsersRoleViewModel usersRoleViewModel, string action)
        {
            if (!ModelState.IsValid)
            {

                if (action == "modify")
                {
                    //Get the role.
                    var role = await _roleManager.FindByIdAsync(usersRoleViewModel.Role.Id);

                    foreach(var userVm in usersRoleViewModel.UsersInRole)
                    // foreach(var identityUser in _userManager.Users)
                    {
                        // var userVm = new UserVM
                        // {
                        //     user = identityUser
                        // };

                        IdentityResult result = null;
                        var user = await _userManager.FindByIdAsync(userVm.user.Id);
                        
                        if (userVm.IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                        {
                            result = await _userManager.AddToRoleAsync(user, role.Name);   
                        }
                        else if (!userVm.IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                        {
                            result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                        }
                        else
                        {
                            continue;
                        }
                    }

                    var roleDashboardVM = new RoleDashboardVM 
                    {
                        Roles = _roleManager.Roles.ToList(),
                        UsersRoleViewModels = new List<UsersRoleViewModel>()
                    };
                    
                    roleDashboardVM.UsersRoleViewModels.Add(usersRoleViewModel);
                }
                else if (action == "delete")
                {
                    //Delete ops.
                    var role = await _roleManager.FindByIdAsync(usersRoleViewModel.Role.Id);
                    try
                    {
                        var result = await _roleManager.DeleteAsync(role);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View("Index");

                    }
                    catch (DbUpdateException ex)
                    {
                        ViewBag.ErrorTitle = $"{role.Name} role is in use.";
                        ViewBag.ErrorMessage = $"{role.Name} role cannot be deleted as there are users in this role.";
                        return View("Index");
                    }
                }
                else if (action == "create")
                {
                    var roleDashboardVM = new RoleDashboardVM();
                    var createRoleVM = new CreateRoleVM();
                    //Create a role.
                    IdentityRole identityRole = new IdentityRole
                    {
                        Name = createRoleVM.RoleName
                    };
                    roleDashboardVM.createRoleVM.RoleName = identityRole.Name;

                    //Save the role in underlying AspNetRoles table.
                    IdentityResult result = await _roleManager.CreateAsync(identityRole);
                }

            }
            return RedirectToAction("ListRoles"); // Return the list to the view, or handle as needed.
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
            var patientDashboardVM = new PatientDashboardVM();
            patientDashboardVM.Patients = _context.User.OfType<Patient>().ToList();
            return View(patientDashboardVM);
        }

        [HttpPost]
        public async Task<IActionResult> Patient(PatientDashboardVM model, string action)
        {

            // Check the action parameter to determine the desired action

            if (action == "delete")
            {
                var patientToDelete = await _context.User.OfType<Patient>().FirstOrDefaultAsync(m => m.Email == model.Email);
                if (patientToDelete != null)
                {
                    //Get all appointments associated with the patient.
                    var appointmentsAssociatedWithPatient =  _context.Appointment.Where(m => m.Patient.Id == patientToDelete.Id).Distinct().ToList();

                    //Loop through appointments deleting them.
                    foreach (var appointment in appointmentsAssociatedWithPatient)
                    {
                        //Delete individual appointment.
                        if (appointment != null)
                        {
                            _context.Appointment.Remove(appointment);
                            _context.SaveChanges();
                        }
                    }

                    _context.Patient.Remove(patientToDelete);
                    _context.SaveChanges();
                    return RedirectToAction("Patient");
                }

                model.Patients = _context.User.OfType<Patient>().ToList();            

            }
            else if (action == "modify")
            {
                // Handle doctor modification logic here
                // Example:
                var existingPatient = await _context.User.OfType<Patient>().FirstOrDefaultAsync(m => m.Email == model.Email);
                if (existingPatient != null)
                {
                    // Update doctor details based on the provided model
                    existingPatient.FirstName = model.FirstName;
                    existingPatient.LastName = model.LastName;
                    existingPatient.Email = model.Email;
                    existingPatient.Nic = model.Nic;
                    existingPatient.PhoneNumber = model.PhoneNumber;

                    _context.Update(existingPatient);
                    await _context.SaveChangesAsync();

                    model.Patient = existingPatient;
                    model.Patients = _context.User.OfType<Patient>().ToList();

                    return RedirectToAction("Index");
                }
            }

            model.Patients = _context.User.OfType<Patient>().ToList();  
            
            return View(model);
        }


        [HttpGet]
        public IActionResult Appointment()
        {
            var administratorDashboardVM = new AdministratorDashboardVM();
            administratorDashboardVM.Appointments = _context.Appointment
                                                            .Include(m=> m.Patient)
                                                            .Include(m=> m.Doctor)
                                                            .ToList();
            return View(administratorDashboardVM);
        }

        [HttpPost]
        public async Task<IActionResult> Appointment(Appointment model, string action)
        {
            if (action == "modify")
            {
                // Get appointment by id.
                var appointment = await _context.Appointment
                    .Include(m => m.Doctor)
                    .Include(m => m.Patient)
                    .FirstOrDefaultAsync(m => m.Id == model.Id);

                // Populate Appointment with new changes.
                if (appointment != null)
                {
                    appointment.Title = model.Title;
                    appointment.AppointmentDate = model.AppointmentDate;
                    appointment.AppointmentTime = model.AppointmentTime;
                }

                // Save changes to the database
                _context.Update(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Appointment");
            }
            else if (action == "delete")
            {
                var appointmentToDelete = await _context.Appointment.FindAsync(model.Id);
                if (appointmentToDelete != null)
                {
                    _context.Appointment.Remove(appointmentToDelete);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("Appointment");
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Settings()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Doctors(DoctorDashboardVM model, string action)
        {
            DoctorDashboardVM doctorDashboardVM = new DoctorDashboardVM();


            // Check the action parameter to determine the desired action
            if (action == "register")
            {
                var existingDoctor = await _userManager.FindByEmailAsync(model.Email);
                if (existingDoctor != null)
                {
                    ModelState.AddModelError("Doctor", "Doctor is already registered");
                    return View(model);
                }

                var doctor = new Doctor
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.FirstName,
                    Email = model.Email,
                    Nic = model.Nic,
                    PhoneNumber = model.PhoneNumber,
                    Speciality = model.Speciality,
                    Password = model.Password,
                };

                doctorDashboardVM.Doctor = doctor;

                var result = await _userManager.CreateAsync(doctor, model.Password);

                //Add the doctor role to a doctor once registered.
                var roleResult = await _userManager.AddToRoleAsync(doctor, "Doctor");

                if (result.Succeeded && roleResult.Succeeded)
                {
                    return RedirectToAction("Doctors");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
            else if (action == "delete")
            {
                // Handle doctor deletion logic here
                // Example:
                var doctorToDelete = await _context.User.OfType<Doctor>().FirstOrDefaultAsync(m => m.Email == model.Email);
                if (doctorToDelete != null)
                {
                    //Get all appointments associated with the doctor.
                    var appointmentsAssociatedWithDoctor =  _context.Appointment.Where(m => m.Doctor.Id == doctorToDelete.Id).Distinct().ToList();

                    //Loop through appointments deleting them.
                    foreach (var appointment in appointmentsAssociatedWithDoctor)
                    {
                        //Delete individual appointment.
                        if (appointment != null)
                        {
                            _context.Appointment.Remove(appointment);
                            _context.SaveChanges();
                        }
                    }

                    //Delete doctor after deleting appointments.
                    if (doctorToDelete != null)
                    {
                        _context.Doctor.Remove(doctorToDelete);
                        _context.SaveChanges();
                    }
                    model.Doctors = _context.User.OfType<Doctor>().ToList();
                    return RedirectToAction("Doctors");
                }
            }
            else if (action == "modify")
            {
                // Handle doctor modification logic here
                // Example:
                var existingDoctor = await _context.User.OfType<Doctor>().FirstOrDefaultAsync(m => m.Email == model.Email);
                if (existingDoctor != null)
                {
                    // Update doctor details based on the provided model
                    existingDoctor.FirstName = model.FirstName;
                    existingDoctor.LastName = model.LastName;
                    existingDoctor.Email = model.Email;
                    existingDoctor.Nic = model.Nic;
                    existingDoctor.PhoneNumber = model.PhoneNumber;
                    existingDoctor.Speciality = model.Speciality;

                    _context.Update(existingDoctor);
                    _context.SaveChanges();

                    model.Doctor = existingDoctor;
                    model.Doctors = _context.User.OfType<Doctor>().ToList();

                    return RedirectToAction("Doctors");
                }
            }
        
            return View(model);
        }
    }
}