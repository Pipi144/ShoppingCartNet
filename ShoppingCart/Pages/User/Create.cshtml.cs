using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingCart.Models;
using ShoppingCart.Services;

namespace ShoppingCart.Pages.User
{
    public class CreateModel : PageModel, IPageModelFunction
    {
        private readonly UserService _userService;

        public CreateModel(UserService userService)
        {
            _userService = userService;
        }

        

        [BindProperty]
        public Models.User User { get; set; } = default!;
        
        [BindProperty]
        public UserRole UserRole { get; set; } = default!;
        
        public List<UserRole> UserRoleOptions { get; set; } = new List<UserRole>
        {
            UserRole.Admin,
            UserRole.Member,
        };
        
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

            UserRole = UserRole.Member;
            return Page();
        }
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            
            if (!ModelState.IsValid)
            {
                return Page();
            }
            User.Role = UserRole;
            _userService.AddUser(User);
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
