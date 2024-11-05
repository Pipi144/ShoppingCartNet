using ShoppingCart.Models;
using ShoppingCart.Repository;

namespace ShoppingCart.Services;

public class ProductService
{
    private readonly ProductRepository _productRepository;
    
    
    public ProductService(ProductRepository productRepository)
    {
        _productRepository = productRepository;
        
    }

    public async Task<List<Product>> GetProducts()
    {
        return await _productRepository.GetAllProducts();
    }

    public List<Product> GetProductByName(string name)
    {
        return _productRepository.GetProductsByName(name);
    }

    public Product? GetProductById(int id)
    {
        return _productRepository.GetProductById(id);
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
        return await _productRepository.DeleteProduct(id);
    }
}