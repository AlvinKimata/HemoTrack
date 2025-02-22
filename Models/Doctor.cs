using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HemoTrack.Models
{
    public class Doctor : User
    {
        [Required]
        public Specialities? Speciality { get; set; }
    }
}
