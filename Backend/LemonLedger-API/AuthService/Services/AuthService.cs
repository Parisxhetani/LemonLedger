using AuthService.DTOs;
using AuthService.Interfaces;
using AuthService.Models;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtGenerator _jwtGenerator;

        public AuthService(UserManager<ApplicationUser> userManager,
                           IJwtGenerator jwtGenerator)
        {
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                return new AuthResponseDto
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };

            var token = _jwtGenerator.GenerateToken(user);
            return new AuthResponseDto { Success = true, Token = token };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null ||
                !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Errors = new List<string> { "Invalid login request" }
                };
            }

            var token = _jwtGenerator.GenerateToken(user);
            return new AuthResponseDto { Success = true, Token = token };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto dto)
        {
            // For MVP, treat refresh as re-login
            return await LoginAsync(new LoginDto
            {
                Email = dto.Email,
                Password = dto.RefreshToken
            });
        }
    }
}
