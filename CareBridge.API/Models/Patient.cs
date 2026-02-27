namespace CareBridge.Api.Models;

public sealed class Patient
{
    public string FamilyName { get; set; } = string.Empty;
    public string GivenName { get; set; } = string.Empty;
    public int Id { get; set; }
    // NOTE: Sentinel Value: Jan 1, 0001
    public DateOnly LastScreeningDate { get; set; } = DateOnly.MinValue;
    public Gender Gender { get; set; } = Gender.Unknown;

    public Patient() { }
}

public enum Gender : byte
{
    Unknown = 0,
    Male = 1,
    Female = 2,
    Other = 3
}
