namespace Movies.API.Requests.Auth;

public class AuthCodeRequest
{
    public string Username { get; set; }
    public string Code { get; set; }
}