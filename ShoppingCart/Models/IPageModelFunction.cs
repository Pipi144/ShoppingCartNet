using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.Models;

public interface IPageModelFunction
{
    Task<IActionResult> OnPostLogoutAsync();
}