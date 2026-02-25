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
        private readonly IEngine _engine;
        private readonly IHubContext<PatientHub> _hubContext;
        private readonly CareBridgeDbContext _dbContext;

        public PatientController(IEngine engine, IHubContext<PatientHub> hubContext, CareBridgeDbContext dbContext)
        {
            _engine = engine;
            _hubContext = hubContext;
            _dbContext = dbContext;
        }

        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetOverduePatients(CancellationToken ct)
        {
            var patientQuery = _dbContext.Patients.AsNoTracking().AsQueryable();

            var filteredQuery = _engine.ApplyScreeningFilter(patientQuery);

            var overdue = await filteredQuery.ToListAsync(ct);

            if (!overdue.Any()) return NotFound();

            return Ok(overdue);
        }

        [HttpPost]
        public async Task<ActionResult<Patient>> AddPatient([FromBody] Patient newPatient, CancellationToken ct)
        {
            _dbContext.Patients.Add(newPatient);
            await _dbContext.SaveChangesAsync();

            var singlePatientQuery = new[] { newPatient }.AsQueryable();

            var isOverdue = _engine.ApplyScreeningFilter(singlePatientQuery).Any();

            if (isOverdue)
            {
                await _hubContext.Clients.All.SendAsync("PatientOverdueAlert", newPatient, ct);
            }

            return CreatedAtAction(nameof(GetOverduePatients), new { id = newPatient.Id }, newPatient);
        }
    }
}
