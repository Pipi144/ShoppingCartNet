using ShoppingCart.Models;
using ShoppingCart.Repository;

namespace ShoppingCart.Services;

public class ProductImageService
{
    private readonly ProductImageRepository _productImageRepository;

    public ProductImageService(ProductImageRepository productImageRepository)
    {
        _productImageRepository = productImageRepository;
    }

    public async Task<IEnumerable<ProductImage>> GetProductImages(int productId)
    {
        return await _productImageRepository.GetProductImagesByProductIdAsync(productId);
    }

    public ProductImage? GetProductImageById(int productImageId)
    {
        return _productImageRepository.GetProductImageById(productImageId);
    }
    public async Task DeleteProductImage(int productImageId)
    {
        await _productImageRepository.RemoveProductImage(productImageId);
    }

    public async Task AddProductImage(ProductImage productImage)
    {
        await _productImageRepository.AddProductImageAsync(productImage);
    }
}