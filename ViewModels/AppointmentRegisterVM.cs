using System.Data;
using HemoTrack.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HemoTrack.ViewModels
{
    public class AppointmentRegisterVM
    {
        public string Title {get; set;}
        public string Email {get; set;} //Get doctor's details with email
        public DateOnly AppointmentDate { get; set; }
        public List<Doctor> Doctors {get; set;}
        public TimeSpan AppointmentTime {get; set;}

        [Required]
        public Patient Patient {get; set;} //Associate a schedule with a doctor

        [Required]
        public Doctor Doctor {get; set;} //Associate a schedule with a patient
    }
}
