using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingCart.Models;
using ShoppingCart.Services;

namespace ShoppingCart.Pages.ProceedCart;

public class Index : PageModel
{
    public Cart CartSession { get; set; }
    public void OnGet()
    {
        CartSession = HttpContext.Session.GetObjectFromJson<Cart>(StorageKeys.CartSessionKey) ?? new Cart(new List<CartItem>());
    }
}