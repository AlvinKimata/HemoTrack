using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HemoTrack.Models
{
    public class Patient : User
    {
        public DateOnly DateOfBirth {get; set;}
        public string Address { get; set; }
    }
}
