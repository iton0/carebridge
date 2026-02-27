namespace CareBridge.Api.Dtos;

// The "Read" DTO
public record PatientDto(
    int Id,
    string FamilyName,
    string GivenName,
    DateOnly? LastScreeningDate,
    string Gender
);
