using HemoTrack.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HemoTrack.ViewModels
{
    public class DoctorDashboardVM
    {
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string Email {get; set;}
        public string Nic {get; set;}
        public string PhoneNumber {get; set;}
        public string Password {get; set;}
        public string Address {get; set;}
        public string DateOfBirth {get; set;}
        public Doctor Doctor {get; set;}
        public List<Patient> Patients {get; set;}
        public List<Doctor> Doctors {get; set;}
        public List<Appointment> Appointments {get; set;}

        public DoctorRegisterVM doctorRegisterVM {get; set;}
        public EditDoctorVM editDoctorVM {get; set;}

        public DoctorDashboardVM()
        {
            doctorRegisterVM = new DoctorRegisterVM();
            editDoctorVM = new EditDoctorVM();    
        }
    }
}
