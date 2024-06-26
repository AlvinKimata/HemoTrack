//Base class for patient and administrator.
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HemoTrack.Models
{
    public class User : IdentityUser
    {  
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password {get; set;}
        public string Nic {get; set;}
        public string? Address {get; set;}
        public string? DateOfBirth {get; set;}
    }
}