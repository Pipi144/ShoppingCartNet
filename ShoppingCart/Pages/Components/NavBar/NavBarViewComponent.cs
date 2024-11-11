using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Models;
using ShoppingCart.Services;

namespace ShoppingCart.Pages.Components.NavBar;

public class NavBarModel
{
    public UserSession? user { get; set; }
    public Cart? cart { get; set; }
}
public class NavBarViewComponent : ViewComponent
{

    
    public NavBarViewComponent()
    {

    }

    public IViewComponentResult Invoke()
    {
        var user = HttpContext.Session.GetObjectFromJson<UserSession>(StorageKeys.UserSessionKey);
        var cart = HttpContext.Session.GetObjectFromJson<Cart>(StorageKeys.CartSessionKey);
        NavBarModel navBarModel = new NavBarModel()
        {
            user = user,
            cart = cart
        };
        return View("Default", navBarModel);
    }

        
    
}
