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

namespace ShoppingCart.Pages.User
{
    public class IndexModel : PageModel, IPageModelFunction
    {
        private readonly UserService _userService;
        private IPageModelFunction _pageModelFunctionImplementation;

        public IndexModel(UserService userService)
        {
            _userService = userService;
        }
        public IList<Models.User> User { get;set; } = default!;

        
        
        public async Task<IActionResult> OnGetAsync()
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
            User = _userService.GetUsers();
            return Page();
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }
    }
}
