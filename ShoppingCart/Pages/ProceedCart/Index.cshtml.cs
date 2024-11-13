using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingCart.Models;
using ShoppingCart.Services;

namespace ShoppingCart.Pages.ProceedCart;

public class Index : PageModel
{
    public Cart CartSession { get; set; }
    public IActionResult OnGet()
    {
        CartSession = HttpContext.Session.GetObjectFromJson<Cart>(StorageKeys.CartSessionKey) ?? new Cart(new List<CartItem>());
        var user = HttpContext.Session.GetObjectFromJson<UserSession>(StorageKeys.UserSessionKey);
        
        // block using cart when user not logged in
        if (user == null)
        {
            return RedirectToPage("/");
        }

        return Page();
    }
    
    public async Task<IActionResult> OnPostRemoveCartItemAsync(int productId)
    {
        try
        {
          
                CartSession = HttpContext.Session.GetObjectFromJson<Cart>(StorageKeys.CartSessionKey) ?? new Cart(new List<CartItem>());

                CartSession.RemoveCartItem(productId);
                //save session
                HttpContext.Session.SetObjectAsJson(StorageKeys.CartSessionKey, CartSession);
            
            return RedirectToPage();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
            
    }
    
    public async Task<IActionResult> OnPostIncreaseQtyAsync(int productId)
    {
        try
        {
          
            CartSession = HttpContext.Session.GetObjectFromJson<Cart>(StorageKeys.CartSessionKey) ?? new Cart(new List<CartItem>());
            var item = CartSession.Items.FirstOrDefault(p=>p.Product.ProductId==productId);
            if (item != null)
            {
                CartSession.AdjustCartQuantity(item, item.Quantity +1);
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
    
    public async Task<IActionResult> OnPostReduceQtyAsync(int productId)
    {
        try
        {
          
            CartSession = HttpContext.Session.GetObjectFromJson<Cart>(StorageKeys.CartSessionKey) ?? new Cart(new List<CartItem>());
            var item = CartSession.Items.FirstOrDefault(p=>p.Product.ProductId==productId);
            if (item != null)
            {
                CartSession.AdjustCartQuantity(item, item.Quantity -1);
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
    
    // the button trigger is in Navbar, but handler is placed here
    public async Task<IActionResult> OnPostLogoutAsync()
    {
        HttpContext.Session.Clear();
        return RedirectToPage("/Index");
    }
}