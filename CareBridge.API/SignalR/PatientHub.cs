using CareBridge.Api.Models;
using Microsoft.AspNetCore.SignalR;

namespace CareBridge.Api.SignalR;

public class PatientHub : Hub
{
    public async Task BroadcastPatients(Patient[] patients)
    {
        await Clients.Others.SendAsync("PatientUpdated", patients);
    }

    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }
}
