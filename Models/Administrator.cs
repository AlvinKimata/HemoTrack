using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace HemoTrack.Models
{
    public class Administrator
    {
        [Key]
        public int AdministratorId { get; set; }
        public string? Email {get; set;}
        public string? password { get; set;}

    }
}
