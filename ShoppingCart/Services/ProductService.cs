using ShoppingCart.Models;
using ShoppingCart.Repository;

namespace ShoppingCart.Services;

public class ProductService
{
    private readonly ProductRepository _productRepository;
    private readonly StorageService _storageService;
    
    public ProductService(ProductRepository productRepository, StorageService storageService)
    {
        _productRepository = productRepository;
        _storageService = storageService;
    }

    public async Task<List<Product>> GetProducts()
    {
        return await _productRepository.GetAllProducts();
    }

    public async Task<List<Product>> GetProductByName(string name)
    {
        return await _productRepository.GetProductsByName(name);
    }

    public async Task<Product?> GetProductById(int id)
    {
        return await _productRepository.GetProductById(id);
    }

    public async Task<Product?> AddProduct(Product product)
    {
        return await _productRepository.AddProduct(product);
    }

    public async Task<Boolean> UpdateProduct(Product product)
    {
        return await _productRepository.UpdateProduct(product);
    }

    public async Task<Boolean> DeleteProduct(int id)
    {
        try
        {
            var res =  await _productRepository.DeleteProduct(id);
            if (res)
            {
                await _storageService.DeleteFolderAsync($"{id}");
            }
            return res;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }
}