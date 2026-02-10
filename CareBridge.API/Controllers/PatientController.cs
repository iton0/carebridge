using CareBridge.Api.Models;
using CareBridge.Api.SignalR;
using CareBridge.Api.Logic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using CareBridge.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CareBridge.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly Engine _engine;
        private readonly IHubContext<PatientHub> _hubContext;
        private readonly CareBridgeDbContext _dbContext;

        public PatientController(Engine engine, IHubContext<PatientHub> hubContext, CareBridgeDbContext dbContext)
        {
            _engine = engine;
            _hubContext = hubContext;
            _dbContext = dbContext;
        }

        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetOverduePatients()
        {
            var allPatients = await _dbContext.Patients.ToListAsync();
            var overdue = await _engine.ScreenAsync(allPatients);
            return Ok(overdue);
        }

        [HttpPost]
        public async Task<ActionResult<Patient>> AddPatient([FromBody] Patient newPatient)
        {
            _dbContext.Patients.Add(newPatient);
            await _dbContext.SaveChangesAsync();

            var allPatients = await _dbContext.Patients.ToListAsync();
            var updatedOverdue = await _engine.ScreenAsync(allPatients);

            await _hubContext.Clients.All.SendAsync("PatientUpdated", updatedOverdue);

            return CreatedAtAction(nameof(GetOverduePatients), new { id = newPatient.Id }, newPatient);
        }
    }
}
