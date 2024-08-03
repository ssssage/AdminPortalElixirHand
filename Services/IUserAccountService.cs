using API.Dtos;

namespace AdminPortalElixirHand.Services
{
    public interface IUserAccountService
    {
        Task<UserDto> LoginAsync(LoginDto loginDto);
        void Logout();
        bool IsLoggedIn { get; }
    }
}
