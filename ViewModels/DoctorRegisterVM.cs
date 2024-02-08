using HemoTrack.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HemoTrack.ViewModels
{
    public class DoctorRegisterVM
    {
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string Email {get; set;}
        public string Nic {get; set;}
        public string PhoneNumber {get; set;}
        public Specialities? Speciality { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password {get; set;}

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage="Password and Confirm Password do not match")]
        public string ConfirmPassword {get; set;}

        // public List<Doctor> Doctors {get; set;}
    }
}
