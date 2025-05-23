using Movies.API.Requests.Auth;
using Movies.Business.Models.Auth;

namespace Movies.Business.Services.Authentication;

public interface IAuthService
{
    Task<AuthenticateResponse?> Login(LoginDTO request);
    Task<AuthenticateResponse?> Register(RegisterDTO request);
    string GenerateJwtToken(string username);
    Task<bool> Is2FAEnabled(string username);
    Task<string> Get2FAQrCode(string username);
    bool ValidateCode(string username, string code);
    Task Enable2FA(string username);
    Task<bool> ConfirmLink(string username, string code);
}