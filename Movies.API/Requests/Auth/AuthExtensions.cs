namespace Movies.API.Requests.Auth;

public static class AuthExtensions
{
    public static LoginDTO ToLoginDTO(this UserLoginRequest userLoginRequest) =>
        new LoginDTO
        {
            Username = userLoginRequest.Username,
            Password = userLoginRequest.Password
        };

    public static RegisterDTO ToRegisterDTO(this UserRegisterRequest userRegisterRequest) =>
        new RegisterDTO
        {
            Username = userRegisterRequest.Username,
            Password = userRegisterRequest.Password,
            Role = userRegisterRequest.Role
        };
}