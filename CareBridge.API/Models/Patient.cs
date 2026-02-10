// Model in MVC
namespace CareBridge.Api.Models;

public record Patient(
    int Id,
    string FamilyName,
    string GivenName,
    DateTime LastScreeningDate,
    string Gender
);
