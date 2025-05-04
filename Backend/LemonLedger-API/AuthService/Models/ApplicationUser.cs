using Microsoft.AspNetCore.Identity;

namespace AuthService.Models
{
    public class ApplicationUser : IdentityUser
    {
        // extra fields for later game logic
        public int CreditScore { get; set; } = 0;
    }
}
