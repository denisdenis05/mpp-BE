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
        
        return Ok(new { Username = response.Username });
    }
    
    [HttpPost("is-2fa-enabled")]
    public async Task<IActionResult> Is2FAEnabled([FromBody] UsernameRequest request)
    {
        return Ok(_authService.Is2FAEnabled(request.Username));
    } 
    
    [HttpPost("get-qr-code")]
    public async Task<IActionResult> GetQrCode([FromBody] UsernameRequest request)
    {
        await _authService.Enable2FA(request.Username);
        
        return Ok(await _authService.Get2FAQrCode(request.Username));
    }
    
    [HttpPost("enable-2fa")]
    public async Task<IActionResult> Enable2FA([FromBody] AuthCodeRequest request)
    {
        return Ok(await _authService.ConfirmLink(request.Username, request.Code));
    }
    
    [HttpPost("validate-2fa")]
    public async Task<IActionResult> Validate2FA([FromBody] AuthCodeRequest request)
    {
        if (_authService.ValidateCode(request.Username, request.Code))
            return Ok(_authService.GenerateJwtToken(request.Username));
        return BadRequest("");
    }
}
