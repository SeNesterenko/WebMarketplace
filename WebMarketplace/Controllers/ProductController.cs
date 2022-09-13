using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebMarketplace.Data;
using WebMarketplace.Models;
using WebMarketplace.ViewModels;

namespace WebMarketplace.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        
        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Product(int goodId, string customerId)
        {
            var good = _db.Products.First(product => product.Id == goodId);
            var queryableSeller = from product in _db.Products where product.Id == goodId select product.User;
            var customer = _db.Users.First(user => user.Id == customerId);
            AppUser seller = null;
            
            foreach (var appUser in queryableSeller)
            {
                seller = appUser;
            }

            var productViewModel = CreateProductViewModel(good, customer, seller);
            return View(productViewModel);
        }

        public async Task<IActionResult> PutTheProductInTheBasket(int quantity, int goodId, string customerId, string sellerId)
        {
            if (quantity == 0) return RedirectToAction("Index", "Home");

            var good = _db.Products.First(product => product.Id == goodId);
            var customer = _db.Users.First(user => user.Id == customerId);
            var seller = _db.Users.First(user => user.Id == sellerId);
            
            if (good.Quantity < quantity || customer.Money < good.Cost * quantity)
            {
                return RedirectToAction("Index", "Home");
            }

            good.Quantity -= quantity;
            customer.Money -= good.Cost * quantity;
            seller.Money += good.Cost * quantity;

            if (good.Quantity == 0)
            {
                _db.Remove(good);
            }
            
            await _db.SaveChangesAsync();
            
            return RedirectToAction("PurchaseWasSuccessful");
        }

        public IActionResult PurchaseWasSuccessful()
        {
            return View();
        }

        private static ProductViewModel CreateProductViewModel(Good good, AppUser customer, AppUser seller)
        {
            var productViewModel = new ProductViewModel
            {
                Good = good,
                Seller = seller,
                Customer = customer
            };

            return productViewModel;
        }
    }
}