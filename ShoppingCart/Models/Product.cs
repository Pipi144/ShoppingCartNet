using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Models;

public class Product
{
    public int ProductId { get; set; }
    
    [Microsoft.Build.Framework.Required]
    [MaxLength(150)]
    [DataType(DataType.Text)]
    public string ProductName { get; set; }

    [Required] public double Price { get; set; } = 0.0;
    
    
    
    public List<ProductImage> ProductImage { get; set; } = new List<ProductImage>();
}