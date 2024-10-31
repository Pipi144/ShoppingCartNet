using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;

namespace ShoppingCart.Repository;

public class UserRepository
{
    private readonly ShoppingCartContext _context;

    public UserRepository(ShoppingCartContext context)
    {
        _context = context;
    }

    public List<User> GetAllUsers()
    {
        return _context.User.ToList();
    }

    public async Task<User?> GetUserById(int id)
    {
        var user = await _context.User.FirstOrDefaultAsync(u => u.Id == id);
        
        return user;
    }

    public void CreateUser(User user)
    {
        _context.User.Add(user);
    }

    public void DeleteUser(int id)
    {
        var user = _context.User.Find(id);
        if (user != null)
        {
            _context.User.Remove(user);
        }
    }

    public User? FindUserByUsername(string username)
    {
        return _context.User.FirstOrDefault(u => u.UserName == username);
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        // Check if the user exists in the database
        var existingUser = await _context.User.FindAsync(user.Id);
        if (existingUser == null)
        {
            // If the user does not exist, return false
            return false;
        }

        // Attach the user to the context and mark as modified
        _context.Entry(existingUser).CurrentValues.SetValues(user);
    
        try
        {
            // Save changes asynchronously
            SaveUserDataAsync();
            return true; // Indicate success
        }
        catch (DbUpdateConcurrencyException)
        {
            // Handle concurrency exceptions if needed
            return false; // Indicate failure
        }
        catch (Exception)
        {
            // Handle other exceptions if needed
            return false; // Indicate failure
        }
    }

    public async void SaveUserDataAsync()
    {
        await _context.SaveChangesAsync();
    }
}