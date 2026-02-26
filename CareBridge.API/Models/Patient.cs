namespace CareBridge.Api.Models;

public sealed class Patient
{
    public int Id { get; set; }
    public string FamilyName { get; set; } = string.Empty;
    public string GivenName { get; set; } = string.Empty;
    public Gender Gender { get; set; } = Gender.Unknown;

    // NOTE: Sentinel Value: Year 0001
    public DateTime LastScreeningDate { get; set; } = DateTime.MinValue;

    public Patient() { }
}

public enum Gender : byte
{
    Unknown = 0,
    Male = 1,
    Female = 2,
    Other = 3
}
