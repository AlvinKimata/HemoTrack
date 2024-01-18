using HemoTrack.Models;
using Microsoft.AspNetCore.Identity;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HemoTrack.ViewModels
{
    public class PatientDashboardVM
    {     
        public int PatientId {get; set;}   
        public string? Email {get; set;}
        public string? FirstName {get; set;}
        public string? UserName {get; set;}
        public List<Doctor> Doctors { get; set; }
        public List<Patient> Patients {get; set;}
        public int PatientCount { get; set; }
         public List<Appointment> Appointments { get; set; }
        public List<Schedule> Schedules { get; set; }
    }
}
