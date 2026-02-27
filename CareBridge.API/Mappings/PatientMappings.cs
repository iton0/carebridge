using CareBridge.Api.Dtos;
using CareBridge.Api.Models;

namespace CareBridge.Api.Mappings;

public static class PatientMappingExtensions
{
    public static PatientDto ToDto(this Patient patient) =>
        new(
            patient.Id,
            patient.FamilyName,
            patient.GivenName,
            patient.LastScreeningDate == DateOnly.MinValue ? null : patient.LastScreeningDate,
            patient.Gender.ToString()
        );
}
