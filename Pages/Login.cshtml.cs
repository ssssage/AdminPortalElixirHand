using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Stripe;

namespace AdminPortalElixirHand.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public void OnGet()
        {

        }

        private readonly List<(string Email, string Password)> _accounts = new List<(string Email, string Password)>
        {
            ("MrTiger@test.com", "Pa$$w0rd"),
            ("MsLion@test.com", "Pa$$w0rd")
        };

        //public async Task<IActionResult> OnPostAsync()
        //{
        //    if (!(Email == "MrTiger@test.com" && Password == "Pas$W0rd"))
        //    {
        //        return Page();
        //    }

        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, "MrTiger"),
        //        new Claim(ClaimTypes.Email, Email),
        //    };

        //    var claimsIdentity = new ClaimsIdentity(
        //        claims,
        //        CookieAuthenticationDefaults.AuthenticationScheme);

        //    await HttpContext.SignInAsync(
        //       CookieAuthenticationDefaults.AuthenticationScheme,
        //       new ClaimsPrincipal(claimsIdentity));

        //    return LocalRedirect(Url.Content("~/"));
        //}

        public async Task<IActionResult> OnPostAsync()
        {
            var account = _accounts.FirstOrDefault(a => a.Email == Email && a.Password == Password);

            if (account == default)
            {
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.Email.Split('@')[0]), // Extract name from email
                new Claim(ClaimTypes.Email, Email),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
               CookieAuthenticationDefaults.AuthenticationScheme,
               new ClaimsPrincipal(claimsIdentity));

            return LocalRedirect(Url.Content("~/"));
        }

    }
}