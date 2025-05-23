using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Movies.API.Requests.Auth;
using Movies.Business.Models.Auth;
using Movies.Data;
using Movies.Data.Models;
using OtpNet;

namespace Movies.Business.Services.Authentication;


public class AuthService : IAuthService
{
    private readonly MovieDbContext _movieDbContext;
    private readonly IConfiguration _configuration;

    public AuthService(MovieDbContext movieDbContext, IConfiguration configuration)
    {
        _movieDbContext = movieDbContext;
        _configuration = configuration;
    }

    public async Task<AuthenticateResponse?> Register(RegisterDTO request)
    {
        var existingUser = await _movieDbContext.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username);
        
        if (existingUser is not null)
            return null;

        var hashedPassword = HashPassword(request.Password);

        var newUser = new User
        {
            Username = request.Username,
            Password = hashedPassword,
            Role = request.Role
        };

        _movieDbContext.Users.Add(newUser);
        await _movieDbContext.SaveChangesAsync();

        return new AuthenticateResponse
        {
            Username = newUser.Username
        };
    }

    public async Task<AuthenticateResponse?> Login(LoginDTO request)
    {
        var user = await _movieDbContext.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username);
        
        if (user is null || !VerifyPassword(request.Password, user.Password))
            return null;
        
        return new AuthenticateResponse
        {
            Username = user.Username
        };
    }

    public async Task<bool> Is2FAEnabled(string username)
    {
        return _movieDbContext.Users.FirstOrDefault(u => u.Username == username).TwoFactorEnabled;
    }

    public async Task Enable2FA(string username)
    {
        var bytes = RandomNumberGenerator.GetBytes(20);
        var userSecretKey = Base32Encoding.ToString(bytes);
        
        var user = _movieDbContext.Users.FirstOrDefault(u => u.Username == username);
        user.TwoFactorSecret = userSecretKey;
        
        await _movieDbContext.SaveChangesAsync();
    }

    public async Task<string> Get2FAQrCode(string username)
    {
        var user = await _movieDbContext.Users.FirstOrDefaultAsync(u => u.Username == username);

        string issuer = "MoviesApp";
        return $"otpauth://totp/{issuer}:{user.Username}?secret={user.TwoFactorSecret}&issuer={issuer}&digits=6";
    }

    public async Task<bool> ConfirmLink(string username, string code)
    {
        if (!ValidateCode(username, code))
            return false;
        
        var user = await _movieDbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        user.TwoFactorEnabled = true;
        
        await _movieDbContext.SaveChangesAsync();

        return true;
    }
    
    public bool ValidateCode(string username, string code)
    {
        var user = _movieDbContext.Users.FirstOrDefault(u => u.Username == username);

        var totp = new Totp(Base32Encoding.ToBytes(user.TwoFactorSecret));
        return totp.VerifyTotp(code, out long _, new VerificationWindow(2, 2));
    }
    
    public string GenerateJwtToken(string username)
    {
        var user = _movieDbContext.Users.FirstOrDefault(u => u.Username == username);
        
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role), 
            new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"]),
            new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"]),
            new Claim(JwtRegisteredClaimNames.Exp,
                new DateTimeOffset(DateTime.UtcNow.AddHours(1)).ToUnixTimeSeconds().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private bool VerifyPassword(string password, string storedHash)
    {
        var hashedPassword = HashPassword(password);
        return hashedPassword == storedHash;
    }
}
