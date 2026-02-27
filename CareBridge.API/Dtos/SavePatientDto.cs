using CareBridge.Api.Models;

namespace CareBridge.Api.Dtos;

public record SavePatientDto(
    string FamilyName,
    string GivenName,
    DateOnly? LastScreeningDate,
    Gender Gender
);
