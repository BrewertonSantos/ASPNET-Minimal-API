using MinimalApiAuth.Models;

namespace MinimalApiAuth.Repositories;

public static class UserRepository
{
    public static UserModel? Get(string username, string password)
    {
        var users = new List<UserModel?>
        {
            new UserModel {UserName = "Brewerton", Password = "a00fb8cb", Role = "manager"},
            new UserModel {UserName = "Thiago", Password = "85c69264", Role = "employee"}
        };

        return users.FirstOrDefault(x => x?.UserName.ToLower() == username.ToLower() && x.Password == password);
    }
}