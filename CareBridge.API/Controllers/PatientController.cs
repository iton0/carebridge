// Controller in MVC
using Microsoft.AspNetCore.Mvc;
using CareBridge.Api.Models;
using CareBridge.Api.Logic;

namespace CareBridge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly Engine _engine;

    public PatientController(Engine engine)
    {
        _engine = engine;
    }

    // Endpoint 1: GET api/patient
    [HttpGet]
    public ActionResult<IEnumerable<Patient>> GetPatients()
    {
        var patients = GetAllPatients();
        return Ok(patients);
    }

    // Endpoint 2: GET api/patient/overdue
    [HttpGet("overdue")]
    public ActionResult<IEnumerable<Patient>> GetOverduePatients()
    {
        var allPatients = GetAllPatients();
        var overdue = _engine.Screen(allPatients);
        return Ok(overdue);
    }

    private List<Patient> GetAllPatients()
    {
        return new List<Patient>
        {
            new Patient(1, "Smith", "John", DateTime.Now.AddYears(-11), "male"),
            new Patient(2, "Garcia", "Maria", DateTime.Now.AddYears(-2), "female"),
            new Patient(3, "Johnson", "Robert", DateTime.Now.AddYears(-12), "male"),
            new Patient(4, "Lee", "Linda", DateTime.Now.AddYears(-1), "female")
        };
    }
}
