using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AdminPortalElixirHand.Services;
using API.Dtos;

namespace AdminPortalElixirHand.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUserAccountService _userAccountService;

        public LoginModel(IUserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        private readonly List<(string Email, string Password)> _accounts = new List<(string Email, string Password)>
        {
            ("MrTiger@test.com", "Pa$$w0rd"),
            ("MsLion@test.com", "Pa$$w0rd")
        };

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Check predefined accounts
            var account = _accounts.FirstOrDefault(a => a.Email == Email && a.Password == Password);
            if (account != default)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.Email.Split('@')[0]), // Extract name from email
                    new Claim(ClaimTypes.Email, Email)
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                   CookieAuthenticationDefaults.AuthenticationScheme,
                   new ClaimsPrincipal(claimsIdentity));

                return LocalRedirect(Url.Content("~/"));
            }

            // If no match in predefined accounts, try to authenticate via API
            var loginDto = new LoginDto
            {
                Email = Email,
                Password = Password
            };

            var user = await _userAccountService.LoginAsync(loginDto);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }

            var apiClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var apiClaimsIdentity = new ClaimsIdentity(
                apiClaims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
               CookieAuthenticationDefaults.AuthenticationScheme,
               new ClaimsPrincipal(apiClaimsIdentity));

            return LocalRedirect(Url.Content("~/"));
        }
    }
}
