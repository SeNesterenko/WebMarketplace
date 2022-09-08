using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using WebMarketplace.Models;

namespace WebMarketplace.ViewModels
{
    public class IndexViewModel
    {
        [Required]
        public string Title { get; set; }
        
        public string? Description { get; set; }
        
        [Required]
        public int Cost { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        public string Picture { get; set; }
        public AppUser User { get; set; }
    }
}