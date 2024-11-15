
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingCart.Services;

namespace ShoppingCart.Pages
{
    public class LoginModel : PageModel
    {
        private readonly UserService _userService;

        public LoginModel(UserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public string UserName { get; set; }
        
        [BindProperty]
        public string Password { get; set; }
        
        
        [BindProperty]
        public string?  ErrorMessage { get; set; }
        public IActionResult OnGet()
        {
            var userSession = HttpContext.Session.GetObjectFromJson<UserSession?>(StorageKeys.UserSessionKey);
            if (userSession != null)
            {
                return RedirectToPage("/Products/Index");
            }
            return Page();
        }

        public IActionResult OnPostAsync()
        {
            var user = _userService.Login(UserName, Password);
            // login failed=> set error message 
            if (user == null)
            {
                ErrorMessage = "Invalid username or password";
                
            }
            // login successfully, set view data to save in session storage => redirect to product list page
            else
            {
                UserSession session = new UserSession()
                {
                    UserName = user.UserName,
                    UserId = user.Id,
                    SessionTime = DateTime.Now.Millisecond,
                    Role = user.Role
                };
                HttpContext.Session.SetObjectAsJson(StorageKeys.UserSessionKey, session);
                ViewData[StorageKeys.UserSessionKey] = user.UserName;
                return RedirectToPage("/Products/Index");
            }

            return Page();
        }
      
    }
}
