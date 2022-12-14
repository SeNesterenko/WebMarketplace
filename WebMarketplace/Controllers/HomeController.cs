using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebMarketplace.Data;
using WebMarketplace.Models;
using WebMarketplace.ViewModels;

namespace WebMarketplace.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db, IWebHostEnvironment appEnvironment)
        {
            _db = db;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Index(IndexViewModel indexViewModel, IFormFile uploadedFile)
        {
            const string permittedExtensions = ".jpg";
            
            if (!ModelState.IsValid || uploadedFile == null) return View(indexViewModel);
            if (!uploadedFile.FileName.Contains(permittedExtensions)) return View(indexViewModel);
            
            var owner = GetUser();
            var path = "/Pictures/" + uploadedFile.FileName;

            await using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(fileStream);
            }
            
            _db.Add(new Good
            {
                Title = indexViewModel.Title,
                Description = indexViewModel.Description,
                Cost = indexViewModel.Cost,
                Quantity = indexViewModel.Quantity,
                Picture = path,
                User = owner
            });
            
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> TopUpBalance(int money)
        {
            var user = GetUser();

            user.Money += money;
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ChangeAvatar(IFormFile uploadedFile)
        {
            const string permittedExtensions = ".jpg";
            
            if (uploadedFile == null) return RedirectToAction("Index", "Home");
            if (!uploadedFile.FileName.Contains(permittedExtensions))  return RedirectToAction("Index", "Home");
            
            var user = GetUser();
            var path = CreateFileAndReturnPath(uploadedFile);
            user.Picture = await path;
            
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
         }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        private AppUser GetUser()
        {
            var claimsPrincipal = User;
            var user = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = _db.Users.First(id => id.Id == user);

            return currentUser;
        }

        private async Task<string> CreateFileAndReturnPath(IFormFile uploadedFile)
        {
            var path = "/Pictures/" + uploadedFile.FileName;

            await using var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create);
            await uploadedFile.CopyToAsync(fileStream);

            return path;
        }
    }
}