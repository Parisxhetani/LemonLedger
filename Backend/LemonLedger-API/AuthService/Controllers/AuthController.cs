using AuthService.DTOs;
using AuthService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var res = await _authService.RegisterAsync(dto);
            if (!res.Success) return BadRequest(res.Errors);
            return Ok(new { token = res.Token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var res = await _authService.LoginAsync(dto);
            if (!res.Success) return Unauthorized(res.Errors);
            return Ok(new { token = res.Token });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenDto dto)
        {
            var res = await _authService.RefreshTokenAsync(dto);
            if (!res.Success) return Unauthorized(res.Errors);
            return Ok(new { token = res.Token });
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var id = User.FindFirst("sub")?.Value;
            var email = User.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
            return Ok(new { id, email });
        }
    }
}
