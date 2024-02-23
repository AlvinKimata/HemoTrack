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
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
                    LastName = "admin",
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

        // [HttpGet]
        // public IActionResult ListRoles()
        // {
        //     var roles = _roleManager.Roles;
        //     var users = _userManager.Users.ToList();
        //     RoleDashboardVM roleDashboardVM = new RoleDashboardVM
        //     {
        //         RoleNames = roles.ToList(),
        //         Users = users
        //     };

        //     return View(roleDashboardVM);
        // }


        [HttpGet]
        public async Task<IActionResult> ListRoles(IdentityRole model)
        {
            //GET method for listing users in a role.

            //Retrieve role from the database.
            var role =  await _roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {model.Id} cannot be found.";
                

                // return View("Index");
            }

            var usersRoleViewModel = new List<UserRoleViewModel>();

            foreach (var user in _userManager.Users.ToList())
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, model.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                usersRoleViewModel.Add(userRoleViewModel);
            }
            RoleDashboardVM roleDashboardVM = new RoleDashboardVM
            {
                UserRoleViewModels = usersRoleViewModel
            };

            return View(roleDashboardVM);
        }
        

        [HttpPost]
        public async Task<IActionResult> ListRoles(List<UserRoleViewModel> userRoleModel, IdentityRole model, string action)
        {
            // Get the role selected.
            var role = await _roleManager.FindByNameAsync(model.Name);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Name = {model.Name} cannot be found";
                return View("Index");
            }

            //Modify action.
            if (action == "modify")
            {
                //Edit operations.
                foreach (var userRole in userRoleModel)
                {
                    int i = 0;

                    var user = await _userManager.FindByIdAsync(userRole.UserId);

                    IdentityResult result = null;

                    if (userRole.IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                    {
                        result = await _userManager.AddToRoleAsync(user, role.Name);
                    }
                    else if (!userRole.IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    else
                    {
                        continue;
                    }

                    if (result != null)
                    {
                        if (i < userRoleModel.Count() - 1)
                            continue;
                        else
                            return RedirectToAction("ListUsers", new { Id = model.Id});
                    }
                    //Increment i.
                    i++;
                }
            }
            else if (action == "delete")
            {
                try
                {
                    var result = _roleManager.DeleteAsync(role).GetAwaiter().GetResult();
                    if (result != null)
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
                    @ViewBag.ErrorTitle = $"{role.Name} role is in use.";
                    @ViewBag.ErrorMessage = $"{role.Name} role cannot be deleted as there" +
                        "are users in this role.";
                    return View("Index");
                }

            }

            return View(userRoleModel); // Return the list to the view, or handle as needed.
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

        [HttpPost]
        public async Task<IActionResult> Patient(Patient model, string action)
        {
            if (!ModelState.IsValid)
            {
                // Check the action parameter to determine the desired action
    
                if (action == "delete")
                {
                    // Handle doctor deletion logic here
                    // Example:
                    var patientToDelete = await _context.User.OfType<Patient>().FirstOrDefaultAsync(m => m.Email == model.Email);
                    if (patientToDelete != null)
                    {
                        _context.Patient.Remove(patientToDelete);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                }
                else if (action == "modify")
                {
                    // Handle doctor modification logic here
                    // Example:
                    var existingPatient = await _context.User.OfType<Doctor>().FirstOrDefaultAsync(m => m.Email == model.Email);
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
                        return RedirectToAction("Index");
                    }
                }
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Appointment()
        {
            var administratorDashboardVM = new AdministratorDashboardVM();
            administratorDashboardVM.Appointments = _context.Appointment.ToList();
            return View(administratorDashboardVM);
        }

        [HttpPost]
        public async Task<IActionResult> Appointment(Appointment model, string action)
        {
            var currentUserName = await GetCurrentPatientAsync();
            var currentUser = _context.User.OfType<Patient>().FirstOrDefault(u => u.UserName == currentUserName.UserName);

            if (action == "create")
            {
                var doctorName = model.Doctor; // Get the name of the doctor
                var doctor = await _context.User.OfType<Doctor>().FirstOrDefaultAsync(m => m.FirstName == doctorName.FirstName);

                var appointment = new Appointment
                {
                    Title = model.Title,
                    AppointmentDate = model.AppointmentDate,
                    AppointmentTime = model.AppointmentTime,
                    Patient = currentUser,
                    Doctor = doctor,
                };

                _context.Appointment.Add(appointment);
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Appointment");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"Unable to add appointment. Error details: {ex.Message}");
                    throw;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Unexpected error occurred while adding appointment. Error details: {ex.Message}");
                    return View(model);
                }

            }
            else if (action == "modify")
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
        public async Task<IActionResult> Doctors(Doctor model, string action)
        {
            if (ModelState.IsValid)
            {
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
                        _context.Doctor.Remove(doctorToDelete);
                        await _context.SaveChangesAsync();
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
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Doctors");
                    }
                }
            }

            return View(model);
        }
    }
}