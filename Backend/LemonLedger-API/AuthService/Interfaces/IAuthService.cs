using AuthService.DTOs;

namespace AuthService.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto refreshDto);
    }
}
