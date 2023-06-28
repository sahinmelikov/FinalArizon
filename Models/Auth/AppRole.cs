using Microsoft.AspNetCore.Identity;

namespace FinalArizon.Models.Auth
{
    public class AppRole : IdentityRole
    {
        public bool IsActivated { get; set; }
    }
}
