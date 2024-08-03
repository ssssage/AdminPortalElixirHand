using API.Dtos;

namespace AdminPortalElixirHand.Services
{
    public class UserAccountService : IUserAccountService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public UserAccountService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public UserDto CurrentUser { get; private set; }

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/account/login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                CurrentUser = await response.Content.ReadFromJsonAsync<UserDto>();
                return CurrentUser;
            }
            return null;
        }

        public void Logout()
        {
            CurrentUser = null;
        }

        public bool IsLoggedIn => CurrentUser != null;
    }
}
