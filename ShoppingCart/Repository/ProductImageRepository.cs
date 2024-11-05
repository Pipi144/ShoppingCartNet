using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;

namespace ShoppingCart.Repository;

public class ProductImageRepository
{
    private readonly ShoppingCartContext _context;

    public ProductImageRepository(ShoppingCartContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<ProductImage>> GetProductImagesAsync()
    {
        return await _context.ProductImage.ToListAsync();
    }

    public async Task<IEnumerable<ProductImage>> GetProductImagesByProductIdAsync(int productId)
    {
        return await _context.ProductImage.Where(p => p.ProductId == productId).ToListAsync();
    }

    public async Task AddProductImageAsync(ProductImage productImage)
    {
        await _context.ProductImage.AddAsync(productImage);
        await SaveImageChanges();
    }

    public  async Task RemoveProductImage(int imageId)
    
    {   
        var productImage = _context.ProductImage.Find(imageId);
        if (productImage != null)
        {
            _context.ProductImage.Remove(productImage);
            await SaveImageChanges();
        }
        
        
    }
    private async Task SaveImageChanges()
    {
        await _context.SaveChangesAsync();
    }
}