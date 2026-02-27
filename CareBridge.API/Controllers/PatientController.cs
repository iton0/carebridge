using CareBridge.Api.Data;
using CareBridge.Api.Dtos;
using CareBridge.Api.Logic;
using CareBridge.Api.Mappings;
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
    [HttpGet("overdue")]
    public async Task<ActionResult<List<PatientDto>>> GetOverduePatients(CancellationToken ct)
    {
        var threshold = ScreeningLogic.GetScreeningThreshold(settings.CutoffDate);

        var patients = await dbContext.Patients
            .AsNoTracking()
            .Where(p => p.LastScreeningDate < threshold)
            .ToListAsync(ct);

        return patients.Select(p => p.ToDto()).ToList();
    }

    [HttpPost]
    public async Task<IResult> AddPatient(SavePatientDto dto, CancellationToken ct)
    {
        // Map DTO back to Entity
        var patient = new Patient
        {
            FamilyName = dto.FamilyName,
            GivenName = dto.GivenName,
            LastScreeningDate = dto.LastScreeningDate ?? DateOnly.MinValue,
            Gender = dto.Gender
        };

        dbContext.Patients.Add(patient);
        await dbContext.SaveChangesAsync(ct);

        var threshold = ScreeningLogic.GetScreeningThreshold(settings.CutoffDate);

        if (ScreeningLogic.IsOverdue(patient.LastScreeningDate, threshold))
        {
            await hubContext.Clients.All.SendAsync("PatientOverdueAlert", new
            {
                patient.Id,
                FullName = $"{patient.FamilyName}, {patient.GivenName}",
                LastDate = patient.LastScreeningDate
            }, ct);
        }

        return Results.Created($"/api/patient/{patient.Id}", patient.ToDto());
    }
}
