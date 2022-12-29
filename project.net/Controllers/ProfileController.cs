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

        [Route("/u/")]
        public IActionResult MyProfile()
        {
            var id = userManager.GetUserId(User);
            return RedirectToAction("Index", new { id });
        }

        [Route("/u/{id}/")]
        public IActionResult Index(string id)
        {
            var user = db.AppUsers.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            var categories = db.Categories
                .Where(c => c.UserId == id)
                .Include("BookmarkCategories")
                .Include("BookmarkCategories.Bookmark");

            ViewBag.user = user;
            ViewBag.categories = categories;
            return View();
        }

        [Route("/u/{userId}/{categoryId}/")]
        public IActionResult ListBookmarks(string userId, int categoryId, [FromQuery(Name = "bookmarkId")] int? bookmarkId)
        {
            var user = db.AppUsers.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return NotFound();

            var bookmarks = db.BookmarkCategories
                .Where(c => c.CategoryId == categoryId)
                .Select(bc => bc.Bookmark);

            bookmarks.Select(x => x.User).Load();
            bookmarks.Select(x => x.Comments).Load();
            bookmarks.Select(x => x.BookmarkCategories).Load();
            bookmarks.Select(x => x.Upvotes).Load();

            foreach (var bk in bookmarks)
                foreach (var comm in bk.Comments)
                    db.Entry(comm).Reference(c => c.User).Load();

            ViewBag.currentBookmark = bookmarks.FirstOrDefault(b => b.Id == bookmarkId);
            ViewBag.categories = db.Categories.Where(c => c.UserId == userId);
            ViewBag.bookmarks = bookmarks.ToList();

            return View();
        }
    }
}
