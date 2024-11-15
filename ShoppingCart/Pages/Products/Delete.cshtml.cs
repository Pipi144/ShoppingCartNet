using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingCart.Models;
using ShoppingCart.Services;

namespace ShoppingCart.Pages.Products
{
    public class DeleteModel : PageModel, IPageModelFunction
    {
        private readonly ProductService _productService;
        private IPageModelFunction _pageModelFunctionImplementation;


        public DeleteModel(ProductService productService)
        {
            _productService = productService;

        }

        [BindProperty]
        public Product Product { get; set; } = default!;
        
        [BindProperty]
        public string ErrorMessage { get; set; } = default!;

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

            var product =await _productService.GetProductById((int)id);

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                Product = product;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var res = await _productService.DeleteProduct((int)id);
                // delete all images from s3
               
                if (res == true)
                
                    return RedirectToPage("./Index");
                else
                {
                    ErrorMessage = "Failed to delete product!";
                    return Page();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ErrorMessage = "Failed to delete product!"+ e.Message;
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
