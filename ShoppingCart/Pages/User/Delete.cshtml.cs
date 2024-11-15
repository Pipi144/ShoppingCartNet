using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingCart.Models;
using ShoppingCart.Services;

namespace ShoppingCart.Pages.User
{
    public class DeleteModel : PageModel, IPageModelFunction
    {
        private readonly UserService _userService;

        public DeleteModel(UserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public Models.User User { get; set; } = default!;

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

            var user =await _userService.GetUserById((int)id);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                User = user;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _userService.DeleteUser((int)id);
            _userService.SaveChanges();

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }
    }
}
