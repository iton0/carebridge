using CareBridge.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CareBridge.Api.Data;

public class CareBridgeDbContext(DbContextOptions<CareBridgeDbContext> options) : DbContext(options)
{
    public DbSet<Patient> Patients => Set<Patient>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var patient = modelBuilder.Entity<Patient>();

        patient.HasData(PatientSeed.GetInitialData());
    }
}
