namespace MinimalApiAuth.Models;

public class UserModel : Model
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public static implicit operator String(UserModel user) => user.UserName;
}