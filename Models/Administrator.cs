using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Microsoft.AspNetCore.Identity;

namespace HemoTrack.Models
{
    public class Administrator
    {
        [Key]
        public string Email {get; set;}
        public string Password { get; set;}

    }
}
