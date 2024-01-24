using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HemoTrack.Models
{
    public class Patient : User
    {
        public DateTime? DateOfBirth {get; set;}
        public string? Address { get; set; }
    }
}
