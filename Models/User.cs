//Base class for patient and administrator.
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HemoTrack.Models
{
    public class User : IdentityUser
    {  
        public int UserId {get; set;}
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        // public string? Email { get; set; } (overridden)
        public string? Password {get; set;}
        public string? Phone { get; set; }
        public string? WebUser {get; set;}
        public string? Nic {get; set;}
    }
}