using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Models;

public enum UserRole
{
    Admin,
    Member
}
public class User
{
    public int Id { get; set; }
    [Microsoft.Build.Framework.Required]
    [MaxLength(100)]
    [DataType("Text")]
    public string UserName { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Required]
    public UserRole Role { get; set; } = UserRole.Member;
    public string? ImageUrl { get; set; }
}