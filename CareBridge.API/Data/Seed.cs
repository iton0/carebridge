using CareBridge.Api.Models;

namespace CareBridge.Api.Data;

public static class PatientSeed
{
    public static Patient[] GetInitialData() =>
    [
        new Patient {
            Id = 1,
            FamilyName = "Smith",
            GivenName = "John",
            LastScreeningDate = new DateTime(2015, 05, 20),
            Gender = Gender.Male
        },
        new Patient {
            Id = 2,
            FamilyName = "Garcia",
            GivenName = "Maria",
            LastScreeningDate = new DateTime(2024, 01, 15),
            Gender = Gender.Female
        },
        new Patient {
            Id = 3,
            FamilyName = "Johnson",
            GivenName = "Robert",
            LastScreeningDate = new DateTime(2014, 11, 10),
            Gender = Gender.Male
        },
        new Patient {
            Id = 4,
            FamilyName = "Lee",
            GivenName = "Linda",
            LastScreeningDate = new DateTime(2025, 02, 01),
            Gender = Gender.Female
        }
    ];
}
