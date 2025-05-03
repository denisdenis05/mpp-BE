using Microsoft.AspNetCore.Mvc;
using Movies.API.Requests.Auth;
using Movies.Business.Services.Authentication;
using LoginRequest = Microsoft.AspNetCore.Identity.Data.LoginRequest;

namespace Movies.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
    {
        var response = await _authService.Register(request.ToRegisterDTO());
        
        if (response is null)
            return BadRequest("User already exists.");
        
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {
        var response = await _authService.Login(request.ToLoginDTO());
        
        if (response is null)
            return Unauthorized("Invalid username or password.");
        
        var token = _authService.GenerateJwtToken(response.Username);
        return Ok(new { Token = token });
    }
}
