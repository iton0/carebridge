namespace CareBridge.Api.Dtos;

public record SavePatientDto(
    string FamilyName,
    string GivenName,
    DateOnly? LastScreeningDate,
    string Gender
);
