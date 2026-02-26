namespace CareBridge.Api.Models;

public class Patient
{
    public int Id { get; set; }
    public string FamilyName { get; set; } = string.Empty;
    public string GivenName { get; set; } = string.Empty;
    public DateTime? LastScreeningDate { get; set; }
    public string Gender { get; set; } = string.Empty;
}
