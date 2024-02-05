using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HemoTrack.Models
{
    public class Appointment
    {
        public string Id { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public TimeSpan AppointmentTime {get; set;}
        public int PatientId {get; set;}
        public List<Patient> Patients {get; set;}
        public int AppointmentNumber { get; set; }
        public int ScheduleId {get; set;}
        public string Title {get; set;}
    }
}
