using HemoTrack.Models;
using System.Data;

namespace HemoTrack.ViewModels
{
    public class AdministratorDashboardVM
    {
        public string Email {get; set;}
        public List<Patient> Patients {get; set;}
        public List<Doctor> Doctors {get; set;}
        public List<Appointment> Appointments {get; set;}
    }
}
