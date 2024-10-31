using Microsoft.Build.Framework;

namespace ShoppingCart.Models;

public class ProductImage
{
    public int ImageId { get; set; }
    
    [Required]
    public string AwsPath { get; set; }
    
    public int ProductId { get; set; }
    
    public Product Product { get; set; }
}