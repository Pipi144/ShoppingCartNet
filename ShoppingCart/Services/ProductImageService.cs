using ShoppingCart.Models;
using ShoppingCart.Repository;

namespace ShoppingCart.Services;

public class ProductImageService
{
    private readonly ProductImageRepository _productImageRepository;
    private readonly StorageService _storageService;

    public ProductImageService(ProductImageRepository productImageRepository, StorageService storageService)
    {
        _productImageRepository = productImageRepository;
        _storageService = storageService;
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
        // Retrieve the image from the database
        var image = _productImageRepository.GetProductImageById(productImageId);
        if (image != null)
        {
            // Delete the image from S3 storage
            await _storageService.DeleteSingleFile(image.AwsPath);

            // Remove the image from the database
            await _productImageRepository.RemoveProductImage(productImageId);
        }


    }

    public async Task AddProductImage(ProductImage productImage)
    {
        
        await _productImageRepository.AddProductImageAsync(productImage);
    }
}