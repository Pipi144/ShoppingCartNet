using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Models;

public class User
{
    public int Id { get; set; }
    [Microsoft.Build.Framework.Required]
    [MaxLength(100)]
    [DataType("Text")]
    public string UserName { get; set; }
    
    [Required]
    public string Password { get; set; }
    public string? ImageUrl { get; set; }
}