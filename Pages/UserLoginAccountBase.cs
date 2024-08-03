using AdminPortalElixirHand.Services;
using API.Dtos;
using Microsoft.AspNetCore.Components;

namespace AdminPortalElixirHand.Pages
{
    public class UserLoginAccountBase : ComponentBase
    {
        [Inject]
        public IUserAccountService UserAccountService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected LoginDto LoginModel = new LoginDto();
        protected bool ShowError;

        protected async Task HandleLogin()
        {
            ShowError = false;
            var userDto = await UserAccountService.LoginAsync(LoginModel);

            if (userDto != null)
            {
                // Store the token and navigate to the main page
                // You might want to use local storage or some state management
                NavigationManager.NavigateTo("/");
                
            }
            else
            {
                ShowError = true;
            }
        }
    }
}
