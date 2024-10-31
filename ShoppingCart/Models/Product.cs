using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Models;

public class Product
{
    public int ProductId { get; set; }
    
    [Microsoft.Build.Framework.Required]
    [MaxLength(150)]
    [DataType(DataType.Text)]
    public string ProductName { get; set; }

    [Required] public decimal Price { get; set; } = 0;
    
    public ICollection<ProductImage> ProductImage { get; set; }

}