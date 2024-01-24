using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.AspNetCore.Identity;

namespace HemoTrack.Models
{
    public class Administrator : IdentityUser
    {
        [Key]
        public int AdministratorId { get; set; }
        public string? Email {get; set;}
        public string? password { get; set;}

    }
}
