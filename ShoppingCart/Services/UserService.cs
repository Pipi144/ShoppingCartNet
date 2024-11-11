using ShoppingCart.Models;
using ShoppingCart.Repository;
using Microsoft.AspNetCore.Http;
namespace ShoppingCart.Services;

public class UserSession 
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public int SessionTime {get; set;}
}
public class UserService
{
    private readonly UserRepository _userRepository;
    
    public UserService(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public List<User> GetUsers()
    {
        return _userRepository.GetAllUsers();
    }

    public async Task<User?> GetUserById(int userId)
    {
        var user =await _userRepository.GetUserById(userId);
        return user;
    }

    public async void DeleteUser(int userId)
    {
        var user =await _userRepository.GetUserById(userId);
        if (user != null)
        {
            _userRepository.DeleteUser(userId);
        }
    }

    public void AddUser(User user)
    {
        user.Role = UserRole.Member;
        _userRepository.CreateUser(user);
    }

    public async Task<Boolean> UpdateUser(User user)
    {
        var res = await _userRepository.UpdateUserAsync(user);
        return res;
    }

    public User? Login(string username, string password)
    {
        var userMatch = _userRepository.FindUserByUsername(username);
        if (userMatch == null || (userMatch.Password != password))
        {
            return null;
        }
        else
        {
            return userMatch;
        }
    }
    

    public void SaveChanges()
    {
        _userRepository.SaveUserDataAsync();
    }
}