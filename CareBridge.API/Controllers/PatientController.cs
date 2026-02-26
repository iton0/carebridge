using CareBridge.Api.Data;
using CareBridge.Api.Logic;
using CareBridge.Api.Models;
using CareBridge.Api.Settings;
using CareBridge.Api.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CareBridge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController(
        IHubContext<PatientHub> hubContext,
        CareBridgeDbContext dbContext,
        ScreeningSettings settings) : ControllerBase
{
    private readonly IHubContext<PatientHub> _hubContext = hubContext;
    private readonly CareBridgeDbContext _dbContext = dbContext;
    private readonly ScreeningSettings _settings = settings;

    [HttpGet("overdue")]
    public async Task<ActionResult<IEnumerable<Patient>>> GetOverduePatients(CancellationToken ct)
    {
        var query = _dbContext.Patients.AsNoTracking();

        var filtered = ScreeningLogic.ApplyFilter(query, _settings.CutoffDate);

        var overdue = await filtered.ToListAsync(ct);

        return overdue.Count != 0 ? Ok(overdue) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<Patient>> AddPatient([FromBody] Patient newPatient, CancellationToken ct)
    {
        _dbContext.Patients.Add(newPatient);
        await _dbContext.SaveChangesAsync(ct);

        if (ScreeningLogic.IsOverdue(newPatient, _settings.CutoffDate))
        {
            await _hubContext.Clients.All.SendAsync("PatientOverdueAlert", newPatient, ct);
        }

        return CreatedAtAction(nameof(GetOverduePatients), new { id = newPatient.Id }, newPatient);
    }
}
