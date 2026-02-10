using Microsoft.EntityFrameworkCore;
using CareBridge.Api.Models;

namespace CareBridge.Api.Data
{
    public class CareBridgeDbContext : DbContext
    {
        public CareBridgeDbContext(DbContextOptions<CareBridgeDbContext> options)
            : base(options) { }

        public DbSet<Patient> Patients { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>().HasData(
                new Patient { Id = 1, FamilyName = "Smith", GivenName = "John", LastScreeningDate = new DateTime(2015, 05, 20), Gender = "male" },
                new Patient { Id = 2, FamilyName = "Garcia", GivenName = "Maria", LastScreeningDate = new DateTime(2024, 01, 15), Gender = "female" },
                new Patient { Id = 3, FamilyName = "Johnson", GivenName = "Robert", LastScreeningDate = new DateTime(2014, 11, 10), Gender = "male" },
                new Patient { Id = 4, FamilyName = "Lee", GivenName = "Linda", LastScreeningDate = new DateTime(2025, 02, 01), Gender = "female" }
            );
        }
    }
}
