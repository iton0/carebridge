using CareBridge.Api.Models;
using CareBridge.Api.Settings;

namespace CareBridge.Api.Logic;

public class Engine : IEngine
{
    private readonly DateTime _cutoff;

    public Engine(ScreeningSettings settings)
    {
        _cutoff = settings.CutoffDate;
    }

    // TODO: make logic more robust
    public IQueryable<Patient> ApplyScreeningFilter(IQueryable<Patient> query)
    {
        // pre-calculate to ensure sql compatibility
        var thresholdDate = _cutoff.AddYears(-1);

        return query.Where(p =>
            p.LastScreeningDate == null ||
            p.LastScreeningDate < thresholdDate
        );
    }
}
