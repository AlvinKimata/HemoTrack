using Microsoft.AspNetCore.SignalR;
using Quartz;
using System;
using HemoTrack.Data;
using HemoTrack.Models;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace HemoTrack.AspNetCoreQuartz.QuartzServices
{
    public class AppointmentJob : IJob
    {
    
        private static string _message = "Appointment message";
        private readonly ILogger<AppointmentJob> _logger;
        private readonly IHubContext<JobsHub> _hubContext;
        private readonly ApplicationDbContext _dbContext;
        private readonly ClaimsPrincipal _currentUser;


        public AppointmentJob()
        {
            // Parameterless constructor
        }

        
        public AppointmentJob(IHubContext<JobsHub> hubContext, ILogger<AppointmentJob> logger, 
                            ApplicationDbContext dbContext)
        {
            _hubContext = hubContext;
            _logger = logger;
            _dbContext = dbContext;
        }

        private async Task<User> GetCurrentPatientAsync()
        {
            string currentUserName = _currentUser.Identity.Name;
            return await _dbContext.User.OfType<Patient>().FirstOrDefaultAsync(u => u.UserName == currentUserName);
        }

        public async Task Execute(IJobExecutionContext context)
        {
            // Get the ID of the currently logged-in patient
            // var currentPatientId = _currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = context.JobDetail.JobDataMap["CurrentPatient"] as Patient;
            var appointments = context.JobDetail.JobDataMap["PatientAppointments"] as IEnumerable<Appointment>;

            if (currentUser != null)
            {

                foreach (var appointment in appointments)
                {
                    // Trigger SignalR event to notify user
                    await _hubContext.Clients.All.SendAsync("ReceiveNotification", $"Upcoming appointment: {appointment.Doctor.FirstName}");
                }
            }

            // var beginMessage = $"This is an {_message} at {DateTime.Now}";
            // // await _hubContext.Clients.All.SendAsync("ConcurrentJobs", beginMessage);
            // // _logger.LogInformation(beginMessage);
            // Console.WriteLine("AppointmentJob Executed at: " + DateTime.Now);
            // // return Task.CompletedTask;

        }
    
    }
    
}