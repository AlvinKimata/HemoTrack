using HemoTrack.Models;

namespace HemoTrack.ViewModels
{
    public class AdministratorDashboardVM
    {
        public string Email {get; set;}
        public List<Patient> Patients {get; set;}
        public List<Doctor> Doctors {get; set;}
        public List<Appointment> Appointments {get; set;}
        public List<Schedule> Schedules {get; set;}
    }
}
