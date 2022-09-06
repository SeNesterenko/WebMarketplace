using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WebMarketplace.Models
{
    public class AppUser : IdentityUser
    {
        public int Money { get; set; }
        public string Picture { get; set; }
        
        public List<Good> Goods { get; set; }
    }
}