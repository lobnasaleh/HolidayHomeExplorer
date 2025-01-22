using Microsoft.AspNetCore.Identity;

namespace MagicVillaApI2.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
