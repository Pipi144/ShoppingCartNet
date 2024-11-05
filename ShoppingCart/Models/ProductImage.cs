using System.Runtime.InteropServices;
using Microsoft.Build.Framework;

namespace ShoppingCart.Models;

public class ProductImage
{
    public int ProductImageId { get; set; }
    
    [Required]
    public string AwsPath { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public bool? IsDefault { get; set; }
    
    public int ProductId { get; set; }
    
    
    public Product Product { get; set; }
}