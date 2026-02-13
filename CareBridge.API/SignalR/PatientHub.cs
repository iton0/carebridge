using Microsoft.AspNetCore.SignalR;
using CareBridge.Api.Models;

namespace CareBridge.Api.SignalR
{
    public class PatientHub : Hub
    {

        public async Task SendPatientUpdates(IEnumerable<Patient> patients)
        {
            await Clients.All.SendAsync("PatientUpdated", patients);
        }

        public override async Task OnConnectedAsync()
        {

            Console.WriteLine($"--> Client Connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }
    }
}
