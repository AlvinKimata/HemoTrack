using HemoTrack.Models;
using Microsoft.AspNetCore.Identity;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HemoTrack.ViewModels
{
    public class RegisterViewModel
    {     
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string? LastName { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string? UserName {get; set;}

        [Required]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth {get; set;}

        [Required]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber {get; set;}

        [Required]
        [Display(Name = "NIC Number")]
        public string? Nic {get; set;}

        [Required]
        [Display(Name = "NIC")]
        public string? Address {get; set;}

    }
}
