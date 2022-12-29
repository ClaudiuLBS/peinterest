using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.net.Data;
using project.net.Models;

namespace project.net.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<AppUser> userManager;

        public ProfileController(
            ApplicationDbContext context,
            IWebHostEnvironment host,
            UserManager<AppUser> userManager
        ) {
            this.db = context;
            this.webHostEnvironment = host;
            this.userManager = userManager;
        }

        [Route("/u/{id}")]
        public IActionResult Index(string id)
        {
            var user = db.AppUsers.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            var categories = db.Categories
                .Where(c => c.UserId == id)
                .Include("BookmarkCategories")
                .Include("BookmarkCategories.Bookmark");

            ViewBag.categories = categories;
            return View();
        }
    }
}
