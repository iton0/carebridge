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
    public async Task<List<Patient>> GetOverduePatients(CancellationToken ct)
    {
        var threshold = ScreeningLogic.GetScreeningThreshold(_settings.CutoffDate);

        return await _dbContext.Patients
            .AsNoTracking()
            .Where(p => p.LastScreeningDate < threshold)
            .ToListAsync(ct);
    }

    [HttpPost]
    public async Task<IResult> AddPatient(Patient patient, CancellationToken ct)
    {
        _dbContext.Patients.Add(patient);
        await _dbContext.SaveChangesAsync(ct);

        var threshold = ScreeningLogic.GetScreeningThreshold(_settings.CutoffDate);

        if (ScreeningLogic.IsOverdue(patient.LastScreeningDate, threshold))
        {
            await _hubContext.Clients.All.SendAsync("PatientOverdueAlert", new
            {
                patient.Id,
                FullName = $"{patient.FamilyName}, {patient.GivenName}",
                LastDate = patient.LastScreeningDate
            }, ct);
        }

        return Results.Created($"/api/patient/{patient.Id}", patient);
    }
}
