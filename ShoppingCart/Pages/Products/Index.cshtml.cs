using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Models;
using ShoppingCart.Services;

namespace ShoppingCart.Pages.Products
{
    public class IndexModel : PageModel, IPageModelFunction
    {
        private readonly ProductService _productService;


        public IndexModel(ProductService productService )
        {
            _productService = productService;
        }
        [BindProperty(SupportsGet = true)]
        public IList<Product> Products { get;set; }
        
        [BindProperty]
        public string SearchString { get; set; } = default!;
        
        public Cart? CartSession { get; set; }
        
        public UserSession? UserSession { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Products = string.IsNullOrEmpty(SearchString) 
                ? await _productService.GetProducts() 
                : await _productService.GetProductByName(SearchString);

            // Ensure Products is initialized as an empty list if null
            Products ??= new List<Product>();
            //initialize cart with session if exist otherwise create new cart instance
            CartSession = HttpContext.Session.GetObjectFromJson<Cart>(StorageKeys.CartSessionKey) ?? new Cart(new List<CartItem>());
            UserSession = HttpContext.Session.GetObjectFromJson<UserSession>(StorageKeys.UserSessionKey);
            
            return Page();
        }
        public async Task<IActionResult> OnPostSearchProductAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchString))
            {
                // Return all products or show a message instead
                Products = await _productService.GetProducts();
            }
            else
            {
                // Search with the provided string
                Products = await _productService.GetProductByName(SearchString);
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAddCartAsync(int productId)
        {
            try
            {
                //if user not logged in we have to direct to log in
                var user = HttpContext.Session.GetObjectFromJson<UserSession>(StorageKeys.UserSessionKey);
                if (user == null)
                {
                    return RedirectToPage("/Login");
                }

                var item = await _productService.GetProductById(productId);
                if (item != null)
                {
                    CartSession = HttpContext.Session.GetObjectFromJson<Cart>(StorageKeys.CartSessionKey) ?? new Cart(new List<CartItem>());

                    CartSession.AddToCart(new CartItem(item,1));
                    //save session
                    HttpContext.Session.SetObjectAsJson(StorageKeys.CartSessionKey, CartSession);
                }
                return RedirectToPage();
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
