using AuthService.Models;

namespace AuthService.Interfaces
{
    public interface IJwtGenerator
    {
        string GenerateToken(ApplicationUser user);
    }
}
