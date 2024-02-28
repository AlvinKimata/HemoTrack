using Microsoft.AspNetCore.SignalR;

namespace HemoTrack.AspNetCoreQuartz;

public class JobsHub : Hub
{
    public Task SendJobMessage(string message)
    {
        return Clients.All.SendAsync("ConcurrentJobs", message);
    }
}