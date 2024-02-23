using System.Data;
using HemoTrack.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HemoTrack.ViewModels
{
    public class AppointmentEditVM
    {
        public int Id { get; set; }
        public string Title {get; set;}
        public string Email {get; set;} //Get doctor's details with email
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime {get; set;}

        public Patient Patient {get; set;} //Associate a schedule with a doctor

        public Doctor Doctor {get; set;} //Associate a schedule with a patient
    }
}
