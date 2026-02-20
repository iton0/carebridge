using CareBridge.Api.Models;

namespace CareBridge.Api.Logic;

public class Engine
{
    // TODO: make logic more robust
    public IQueryable<Patient> ApplyScreeningFilter(IQueryable<Patient> query)
    {
        var cutoffDate = DateTime.Today.AddYears(-1);

        return query
            .Where(p => p.LastScreeningDate == null || p.LastScreeningDate < cutoffDate)
            .Select(p => new Patient
            {
                Id = p.Id,
                FamilyName = p.FamilyName,
                GivenName = p.GivenName,
                LastScreeningDate = p.LastScreeningDate
            });
    }
}
