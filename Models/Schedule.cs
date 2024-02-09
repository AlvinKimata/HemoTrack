using Microsoft.AspNetCore.Mvc.Rendering;
using Org.BouncyCastle.Asn1.Cms;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HemoTrack.Models
{
    public class Schedule
    {
        public string Id { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public string Title {get; set;}
        public TimeSpan? ScheduleTime { get; set; }
        public int Nop {get; set;}
        public string DoctorId {get; set;} //Associate a schedule with a doctor
        public string PatientId {get; set;} //Associate a schedule with a patient
    }
}
