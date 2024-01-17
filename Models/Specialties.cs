using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace HemoTrack.Models
{
    public class Specialities
    {
        [Key]
        public int SpecialityId { get; set; }
        public string? SpecialityName { get; set;}

    }
}
