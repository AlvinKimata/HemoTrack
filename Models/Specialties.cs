using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace HemoTrack.Models
{
    public class Specialities
    {

        public int SpecialityId { get; set; }


        public string? SpecialityName { get; set;}
        public string? Description { get; set; }

    }
}
