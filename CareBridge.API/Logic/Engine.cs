using CareBridge.Api.Models;

namespace CareBridge.Api.Logic;

public class Engine
{
    public async Task<List<Patient>> ScreenAsync(List<Patient> patients)
    {
        return await Task.Run(() =>
            patients.Where(p => (DateTime.Today - p.LastScreeningDate).TotalDays > 365).ToList()
        );
    }
}
