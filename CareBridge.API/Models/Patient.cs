// Model in MVC
namespace CareBridge.Api.Models;

// TODO: HL7 FHIR Compliance](https://www.hl7.org/fhir/patient.html)
public record Patient
{
    public int Id { get; init; }
    public string FamilyName { get; init; } = string.Empty;
    public string GivenName { get; init; } = string.Empty;
    public DateTime LastScreeningDate { get; init; }
    public string Gender { get; init; } = string.Empty;

    public Patient() { }

    public Patient(int id, string family, string given, DateTime last, string gender)
    {
        Id = id;
        FamilyName = family;
        GivenName = given;
        LastScreeningDate = last;
        Gender = gender;
    }
}
