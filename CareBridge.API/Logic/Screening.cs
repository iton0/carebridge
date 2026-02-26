using CareBridge.Api.Models;

namespace CareBridge.Api.Logic;

public static class ScreeningLogic
{
    private static readonly DateTime DateFloor = new(1700, 1, 1);

    public static IQueryable<Patient> ApplyFilter(IQueryable<Patient> query, DateTime cutoffDate)
    {
        if (cutoffDate < DateFloor)
        {
            return query.Where(p => false);
        }

        var threshold = cutoffDate.AddYears(-1);

        return query.Where(p => p.LastScreeningDate == null || p.LastScreeningDate < threshold);
    }

    public static bool IsOverdue(Patient p, DateTime cutoffDate)
    {
        if (cutoffDate < DateFloor) return false;

        var threshold = cutoffDate.AddYears(-1);
        return p.LastScreeningDate == null || p.LastScreeningDate < threshold;
    }
}
