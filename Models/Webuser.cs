using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace HemoTrack.Models
{
    public class Webuser
    {
        [Key]
        public int WebuserId { get; set; }
        public string? Email {get; set;}
        public string? usertype { get; set;}

    }
}
