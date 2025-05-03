namespace Movies.API.Requests.Auth;

public class RegisterDTO
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}