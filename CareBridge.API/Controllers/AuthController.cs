using CareBridge.Api.Dtos;
using CareBridge.Api.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareBridge.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(JwtService jwtService) : ControllerBase
{
    private readonly JwtService _jwtService = jwtService;

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        if (dto.Username == "admin" && dto.Password == "password")
        {
            var token = _jwtService.GenerateToken(dto.Username);
            return Ok(new { Token = token });
        }

        return Unauthorized();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("secure-data")]
    public IActionResult GetSecureData()
    {
        return Ok("This is protected data.");
    }
}
