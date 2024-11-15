using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;
using ShoppingCart.Services;

namespace ShoppingCart.Pages.Products
{
    public class EditModel : PageModel, IPageModelFunction
    {
        private readonly ProductService _productService;
        private readonly ProductImageService _productImageService;
        private readonly StorageService _storageService;
        private IPageModelFunction _pageModelFunctionImplementation;

        public EditModel(ProductService productService, ProductImageService productImageService, StorageService storageService)
        {
            _productService = productService;
            _productImageService = productImageService;
            _storageService = storageService;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;
        
        public string ErrorMessage { get; set; } = "";
        [BindProperty] public List<IFormFile> UploadedFiles { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var userSession = HttpContext.Session.GetObjectFromJson<UserSession?>(StorageKeys.UserSessionKey);
            //prevent access page if user not logged in
            if (userSession == null)
            {
                return RedirectToPage("/Login");
            }
            // block member to use this function
            else if (userSession.Role == UserRole.Member)
            {
                return RedirectToPage("/Index");
            }
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductById(id.Value);
            if (product == null)
            {
                return NotFound();
            }
            Product = product;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var res = await _productService.UpdateProduct(Product);
            if (res == true)
            {
                if (UploadedFiles.Count <= 0) return RedirectToPage("./Index");
                // Iterate over each file
                foreach (var (formFile, index) in UploadedFiles.Select((value, i) => (value, i)))
                {
                    if (formFile.Length > 0)
                    {

                        var resUpload = await _storageService.AddFileS3(formFile,
                            $"{Product.ProductId}"); // add prefix is product id to easily delete the whole product after

                        // if successfully upload => add image info to db
                        if (resUpload != null)
                        {
                            var imgObject = new ProductImage()
                            {
                                ProductId = Product.ProductId,
                                AwsPath = resUpload,
                            };
                            if (index == 0) imgObject.IsDefault = true; // set default for first image
                            await _productImageService.AddProductImage(imgObject);

                        }
                    }
                }
                return RedirectToPage("./Index");
            }
            else
            {
                ErrorMessage = "Failed to update product";
                return Page();

            }
        }

        public async Task<IActionResult> OnPostRemoveImageAsync(int productImageId)
        {
            var productId = Product.ProductId;
            try
            {
                await _productImageService.DeleteProductImage(productImageId);
                return RedirectToPage(new {id = productId});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorMessage = "Failed to delete image";
                return Page();
            }
            
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }
    }
}
