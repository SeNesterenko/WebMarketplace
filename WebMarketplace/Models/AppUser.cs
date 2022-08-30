using Microsoft.AspNetCore.Identity;

namespace WebMarketplace.Models
{
    public class AppUser : IdentityUser
    {
        public string NickName { get; set; }
    }
}