using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;
using ShoppingCart.Services;
using Sprache;

namespace ShoppingCart.Repository;

public class ProductRepository
{
    private readonly ShoppingCartContext _context;
    private readonly StorageService _storageService;
    
    
    public ProductRepository(ShoppingCartContext context, StorageService storageService)
    {
        _context = context;
        _storageService = storageService;
    }

    public async Task<List<Product>> GetAllProducts()
    {
        var products = await _context.Product
            .Include(p => p.ProductImage) // Eager load images with products
            .ToListAsync();

        foreach (var product in products)
        {
            foreach (var image in product.ProductImage)
            {
                image.ImageUrl = _storageService.GeneratePreSignedUrl(
                    image.AwsPath,
                    TimeSpan.FromHours(1) // Set your desired expiration duration
                );
            }
        }

        return products;
    }

    public async Task<Product?> GetProductById(int id)
    {
        var product = await _context.Product
            .Include(p => p.ProductImage) // Eager load images with products
            .FirstOrDefaultAsync(p => p.ProductId == id);

        if (product != null)
        {
            foreach (var image in product.ProductImage)
            {
                image.ImageUrl = _storageService.GeneratePreSignedUrl(
                    image.AwsPath,
                    TimeSpan.FromHours(1)
                );
            }
        }

        return product;
    }

    public async Task<List<Product>> GetProductsByName(string name)
    {
        return await _context.Product
            .Where(product => product.ProductName.ToLower().Contains(name.ToLower()))
            .ToListAsync();
    }

    public async Task<Boolean> DeleteProduct(int id)
    {
        var product = await GetProductById(id);
        if (product == null) return false;

        try
        {
            _context.Product.Remove(product);
            await SaveProductChanges();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error deleting product:", e);
            return false;
        }
    }

    public async Task<Product?> AddProduct(Product product)
    {
        try
        {
            _context.Product.Add(product);
            await SaveProductChanges();
            return product;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
        
    }

    public async Task<Boolean> UpdateProduct(Product product)
    {
        var existingProduct = await GetProductById(product.ProductId);
        if (existingProduct == null) return false;
        
        try
        {
            _context.Entry(existingProduct).CurrentValues.SetValues(product);
            await SaveProductChanges();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
    }
    private async Task SaveProductChanges()
    {
        await _context.SaveChangesAsync();
    }
}