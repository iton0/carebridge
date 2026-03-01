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

    public static Patient ToEntity(this SavePatientDto dto) =>
        new()
        {
            FamilyName = dto.FamilyName,
            GivenName = dto.GivenName,
            LastScreeningDate = dto.LastScreeningDate ?? DateOnly.MinValue,
            Gender = MapToGender(dto.Gender)
        };

    private static Gender MapToGender(string? input) =>
        input?.Trim().ToLower() switch
        {
            "male" or "m" or "1" => Gender.Male,
            "female" or "f" or "2" => Gender.Female,
            "other" or "o" or "3" => Gender.Other,
            _ => Gender.Unknown
        };
}
