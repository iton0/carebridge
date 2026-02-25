using CareBridge.Api.Models;

namespace CareBridge.Api.Logic;

public interface IEngine
{
    IQueryable<Patient> ApplyScreeningFilter(IQueryable<Patient> query);
}
