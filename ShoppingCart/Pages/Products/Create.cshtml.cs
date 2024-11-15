using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingCart.Models;
using ShoppingCart.Services;

namespace ShoppingCart.Pages.Products
{
    public class CreateModel : PageModel, IPageModelFunction
    {
        private readonly ProductService _productService;
        private readonly ProductImageService _productImageService;
        private readonly StorageService _storageService;
        private IPageModelFunction _pageModelFunctionImplementation;

        public CreateModel(ProductService productService, ProductImageService productImageService, StorageService storageService)
        {
            _productService = productService;
            _productImageService = productImageService;
            _storageService = storageService;
        }

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

            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        public string ErrorDisplayMessage { get; set; } = "";
        [BindProperty] public List<IFormFile> UploadedFiles { get; set; }
        [BindProperty] public bool IsAdding { get; set; } = false;
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                IsAdding = true;
                if (!ModelState.IsValid)
                {
                    foreach (var state in ModelState)
                    {
                        if (state.Value.Errors.Count > 0)
                        {
                            foreach (var error in state.Value.Errors)
                            {
                                Console.WriteLine($"Error in {state.Key}: {error.ErrorMessage}");
                            }
                        }
                    }
                    return Page();
                }
                // add the product general details first to get the product id
                var res = await _productService.AddProduct(Product);
                
                // only continue when add db success
                if (res != null)
                {
                    // uploading images to s3 and get the file key
                    if (UploadedFiles.Count > 0)
                    {
                        int errorFilesCount = 0;

                        // Iterate over each file
                        foreach (var (formFile, index) in UploadedFiles.Select((value,i)=>(value,i)))
                        {
                            if (formFile.Length > 0)
                            {
                                
                                var resUpload = await _storageService.AddFileS3(formFile, 
                                    $"{res.ProductId}");// add prefix is product id to easily delete the whole product after

                                // if successfully upload => add image info to db
                                if (resUpload != null)
                                {
                                    var imgObject = new ProductImage()
                                    {
                                        ProductId = res.ProductId,
                                        AwsPath = resUpload,
                                    };
                                    if (index==0) imgObject.IsDefault = true; // set default for first image
                                    await _productImageService.AddProductImage(imgObject);
                                   
                                }
                                else errorFilesCount++;

                            }
                        }


                        if (errorFilesCount > 0)
                        {
                            ErrorDisplayMessage = "Some Error Files";
                        }
                    }
                    return RedirectToPage("./Index");
                }
               
                else
                {
                    ErrorDisplayMessage = "Something went wrong.";
                    return Page();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }
    }
}
