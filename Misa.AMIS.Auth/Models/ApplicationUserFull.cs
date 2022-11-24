using Microsoft.AspNetCore.Identity;

namespace Misa.AMIS.Auth.Models
{
    public class ApplicationUserFull:IdentityUser
    {
        public string? FullName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
