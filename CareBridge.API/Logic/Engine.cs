using CareBridge.Api.Models;

namespace CareBridge.Api.Logic;

public class Engine
{
    public List<Patient> Screen(List<Patient> patients)
    {
        return patients.Where(p => (DateTime.Today - p.LastScreeningDate).TotalDays > 365).ToList();
    }
}
