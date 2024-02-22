using System.Data;
using HemoTrack.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HemoTrack.ViewModels
{
    public class AppointmentDashboardVM
    {
        public string Id {get; set;}
        public List<Doctor> Doctors {get; set;}

        public Patient Patient {get; set;} //Associate a schedule with a doctor

        public Doctor Doctor {get; set;} //Associate a schedule with a patient

        public AppointmentRegisterVM appointmentRegisterVM {get; set;}

        public AppointmentDashboardVM()
        {
            appointmentRegisterVM = new  AppointmentRegisterVM();
        }
        
    }
}