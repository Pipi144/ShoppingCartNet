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
            
            .ToListAsync();
        foreach (var product in products)
        {
            product.ProductImage = _context.ProductImage.Where(img => img.ProductId == product.ProductId)
                .Select(img => new ProductImage()
                {
                    ProductImageId = img.ProductImageId,
                    AwsPath = img.AwsPath,
                    ImageUrl =
                        _storageService.GeneratePreSignedUrl(
                            img.AwsPath,
                            TimeSpan.FromHours(1) // Set your desired expiration duration
                        )
                }).ToList();
        };
        Console.WriteLine("PRODUCT:", products);

        return products;
    }

    public Product? GetProductById(int id)
    {
        return _context.Product.FirstOrDefault(product => product.ProductId == id);
    }

    public List<Product> GetProductsByName(string name)
    {
        return _context.Product.Where(product => product.ProductName.ToLower().Contains(name.ToLower())).ToList();
    }

    public async Task<Boolean> DeleteProduct(int id)
    {
        var product = GetProductById(id);
        if (product == null) return false;
        try
        {
            _context.Product.Remove(product);
            await SaveProductChanges();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
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
        var products = GetProductById(product.ProductId);
        try
        {
            _context.Attach(product).State = EntityState.Modified;
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