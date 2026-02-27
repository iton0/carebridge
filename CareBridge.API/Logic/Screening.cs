namespace CareBridge.Api.Logic;

using CareBridge.Api.Models;

public static class ScreeningLogic
{
    public static DateOnly GetScreeningThreshold(DateOnly cutoffDate)
    {
        return cutoffDate.AddYears(-1);
    }

    public static bool IsOverdue(DateOnly lastScreening, DateOnly threshold)
    {
        return lastScreening < threshold;
    }

    public static IQueryable<Patient> ApplyFilter(IQueryable<Patient> query, DateOnly cutoffDate)
    {
        var threshold = GetScreeningThreshold(cutoffDate);

        return query.Where(p => p.LastScreeningDate < threshold);
    }
}
