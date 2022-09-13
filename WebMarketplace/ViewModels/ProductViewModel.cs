using System.ComponentModel.DataAnnotations;
using WebMarketplace.Models;

namespace WebMarketplace.ViewModels
{
    public class ProductViewModel
    {
        [Required]
        public int Quantity;
        
        public AppUser Seller;
        public AppUser Customer;
        public Good Good;
    }
}