using CareBridge.Api.Models;
using CareBridge.Api.SignalR;
using CareBridge.Api.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CareBridge.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly Engine _engine;
        private readonly IHubContext<PatientHub> _hubContext;

        // Static list simulates a persistent database for the session
        private static readonly List<Patient> _patients = new List<Patient>
        {
            new Patient(1, "Smith", "John", DateTime.Now.AddYears(-11), "male"),
            new Patient(2, "Garcia", "Maria", DateTime.Now.AddYears(-2), "female"),
            new Patient(3, "Johnson", "Robert", DateTime.Now.AddYears(-12), "male"),
            new Patient(4, "Lee", "Linda", DateTime.Now.AddYears(-1), "female")
        };

        public PatientController(Engine engine, IHubContext<PatientHub> hubContext)
        {
            _engine = engine;
            _hubContext = hubContext;
        }

        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetOverduePatients()
        {
            var overdue = await _engine.ScreenAsync(_patients);
            return Ok(overdue);
        }

        [HttpPost]
        public async Task<ActionResult<Patient>> AddPatient([FromBody] Patient newPatient)
        {
            _patients.Add(newPatient);

            // Calculate the new overdue list after the addition
            var updatedOverdue = await _engine.ScreenAsync(_patients);

            // Broadcast the new overdue list to ALL connected Angular clients
            await _hubContext.Clients.All.SendAsync("PatientUpdated", updatedOverdue);

            return CreatedAtAction(nameof(GetOverduePatients), new { id = newPatient.Id }, newPatient);
        }
    }
}
