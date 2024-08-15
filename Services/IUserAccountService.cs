using API.Dtos;

namespace AdminPortalElixirHand.Services
{
    public interface IUserAccountService
    {
        Task<UserDto> LoginAsync(LoginDto loginDto);

        Task<UserDto> RegisterAsync(RegisterDto registerDto);
        void Logout();
        bool IsLoggedIn { get; }
    }
}
