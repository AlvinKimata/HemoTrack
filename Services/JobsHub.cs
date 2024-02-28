using Microsoft.AspNetCore.SignalR;

namespace HemoTrack.Services;

public class JobsHub : Hub
{
    public Task SendJobMessage(string message)
    {
        return Clients.All.SendAsync("ConcurrentJobs", message);
    }
}