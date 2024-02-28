using Microsoft.AspNetCore.SignalR;
using Quartz;
// using HemoTrack.Models;

namespace HemoTrack.Services
{
    public class AppointmentJob : IJob
    {
    
        private static string _message = "Appointment message";
        private readonly ILogger<ConconcurrentJob> _logger;
        private readonly IHubContext<JobsHub> _hubContext;

        public AppointmentJob(IHubContext<JobsHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var beginMessage = $"This is an {_message} at {DateTime.Now}";
            await _hubContext.Clients.All.SendAsync("ConcurrentJobs", beginMessage);
            _logger.LogInformation(beginMessage);
        }
    
    }
    
}