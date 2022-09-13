using System.Collections.Generic;

namespace WebMarketplace.Models
{
    public class Basket
    {
        public int Id { get; set; }        
        public List<Good> Goods { get; set; }
    }
}