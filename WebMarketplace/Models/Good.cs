namespace WebMarketplace.Models
{
    public class Good
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int Cost { get; set; }
        public int Quantity { get; set; }
        public string Picture { get; set; }
        
        public AppUser User { get; set; }
    }
}