namespace CareBridge.Api.Dtos;

public record PatientDto(
    int Id,
    string FamilyName,
    string GivenName,
    DateOnly? LastScreeningDate,
    string Gender
);
