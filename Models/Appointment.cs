using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HemoTrack.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentNumber { get; set; }
        public string Title {get; set;}
        public DateTime? AppointmentDate { get; set; }
        public TimeSpan AppointmentTime {get; set;}
        public Patient Patient {get; set;} //Associate a schedule with a doctor
        public Doctor Doctor {get; set;} //Associate a schedule with a patient
    }
}
