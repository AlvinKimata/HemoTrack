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
            var currentUser = _context.User.OfType<Patient>().FirstOrDefault(u => u.UserName == currentUserName);
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
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;
            RoleDashboardVM roleDashboardVM = new RoleDashboardVM
            {
                RoleNames = roles.ToList()
            };

            return View(roleDashboardVM);
        }


        [HttpGet]
        public List<User> ListUsersInRole(string id)
        {
            // Find the role by Role ID
            var role =  Task.Run(async () => await _roleManager.FindByIdAsync(id)).GetAwaiter().GetResult();

            // if (role == null)
            // {
            //     return NotFound($"Role with Id = {id} cannot be found");
            // }

            List<User> UsersInRole = new List<User>();

            // Retrieve all the Users
            foreach (var user in _userManager.Users)
            {
                // For each user, the IsInRoleAsync method of the user manager is used to check if the user is in the role.
                // If the user is in the role, they are added to the UsersInRole list.
                var isInRole = Task.Run(async () => await _userManager.IsInRoleAsync(user, role.Name)).GetAwaiter().GetResult();

                if (isInRole)
                {
                    UsersInRole.Add(user);
                }
            }

            return UsersInRole;
        }


        
        // Role ID is passed from the URL to the action
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            // Find the role by Role ID
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleVM
            {
                Id = role.Id,
                RoleName = role.Name
            };

            // Retrieve all the Users
            foreach (var user in _userManager.Users)
            {
                // If the user is in this role, add the username to
                // Users property of EditRoleViewModel. This model
                // object is then passed to the view for display
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        // This action responds to HttpPost and receives EditRoleViewModel
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleVM model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;

                // Update the Role using UpdateAsync
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
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
            if (ModelState.IsValid)
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

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            //Retrieve role from the database.
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {roleId} cannot be found.";
                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>();

            foreach (var user in _userManager.Users.ToList())
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id: {roleId} cannot be found.";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }
            }
            return RedirectToAction("EditRole", new { id = roleId });
        }

        [HttpGet]
        public IActionResult EditDoctor()
        {
            return View();
        }

    }
}