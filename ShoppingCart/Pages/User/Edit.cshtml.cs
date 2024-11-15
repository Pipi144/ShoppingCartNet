using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingCart.Models;
using ShoppingCart.Services;

namespace ShoppingCart.Pages.User
{
    public class EditModel : PageModel, IPageModelFunction
    {
        private readonly UserService _userService;
        private IPageModelFunction _pageModelFunctionImplementation;

        public EditModel(UserService userService)
        {
            _userService = userService;
        }

        [BindProperty] public Models.User User { get; set; } = default!;

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

            var user = await _userService.GetUserById(id.Value);
            if (user == null)
            {
                return NotFound();
            }

            User = user;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var res = await _userService.UpdateUser(User);
            if (!res)
            {
                // Handle failure (e.g., user not found or concurrency issue)
                ModelState.AddModelError("", "Unable to update user.");
                return Page();
            }

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Index");
        }
    }
}