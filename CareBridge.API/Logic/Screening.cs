namespace CareBridge.Api.Logic;

using CareBridge.Api.Models;

public static class ScreeningLogic
{
    public static DateTime GetScreeningThreshold(DateTime cutoffDate)
    {
        return cutoffDate.AddYears(-1);
    }

    public static bool IsOverdue(DateTime lastScreening, DateTime threshold)
    {
        return lastScreening < threshold;
    }

    public static IQueryable<Patient> ApplyFilter(IQueryable<Patient> query, DateTime cutoffDate)
    {
        var threshold = GetScreeningThreshold(cutoffDate);

        return query.Where(p => p.LastScreeningDate < threshold);
    }
}
