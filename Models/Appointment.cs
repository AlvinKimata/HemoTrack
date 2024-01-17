using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HemoTrack.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public int AppointmentNumber { get; set; }
        public int ScheduleId {get; set;}
    }
}
