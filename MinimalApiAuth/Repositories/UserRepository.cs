using MinimalApiAuth.Models;

namespace MinimalApiAuth.Repositories;

public static class UserRepository
{
    public static UserModel? Get(string username, string password)
    {
        var users = new List<UserModel?>
        {
            new UserModel {UserName = "Brewerton", Password = "a00fb8cb"},
            new UserModel {UserName = "Thiago", Password = "85c69264"}
        };

        return users.FirstOrDefault(x => x?.UserName.ToLower() == username.ToLower() && x.Password == password);
    }
}