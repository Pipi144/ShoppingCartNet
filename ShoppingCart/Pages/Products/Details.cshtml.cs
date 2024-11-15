using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingCart.Models;
using ShoppingCart.Services;

namespace ShoppingCart.Pages.Products
{
    public class DetailsModel : PageModel, IPageModelFunction
    {
        private readonly ProductService _productService;

        public DetailsModel(ProductService productService)
        {
            _productService = productService;
        }

        public Product Product { get; set; } = default!;

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
            else
            {
                Product = product;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }
    }
}
