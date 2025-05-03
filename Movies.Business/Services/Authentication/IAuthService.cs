using Movies.API.Requests.Auth;
using Movies.Business.Models.Auth;

namespace Movies.Business.Services.Authentication;

public interface IAuthService
{
    Task<AuthenticateResponse?> Login(LoginDTO request);
    Task<AuthenticateResponse?> Register(RegisterDTO request);
    string GenerateJwtToken(string username);
}