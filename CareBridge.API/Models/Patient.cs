// Model in MVC
namespace CareBridge.Api.Models;

// TODO: HL7 FHIR Compliance](https://www.hl7.org/fhir/patient.html)
public record Patient(
    int Id,
    string FamilyName,
    string GivenName,
    DateTime LastScreeningDate,
    string Gender
);
