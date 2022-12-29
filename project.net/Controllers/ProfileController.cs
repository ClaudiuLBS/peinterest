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

            var userPersonalBookmark = db.Bookmarks
                .OrderByDescending(b => b.CreatedAt)
                .FirstOrDefault(b => b.UserId == id);

            ViewBag.userPersonalBookmark = userPersonalBookmark;
            ViewBag.user = user;
            ViewBag.categories = categories;
            return View();
        }

        public dynamic GetViewBag()
        {
            return ViewBag;
        }

        [Route("/u/{userId}/{categoryId}/")]
        public IActionResult ListBookmarks(string userId, int categoryId, [FromQuery(Name = "bookmarkId")] int? bookmarkId, dynamic viewBag)
        {
            var user = db.AppUsers.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return NotFound();
            
            var bookmarks = db.BookmarkCategories
                .Where(c => c.CategoryId == categoryId)
                .Select(bc => bc.Bookmark)
                .OrderByDescending(b => b.CreatedAt);

            if (categoryId == 0)
            {
                bookmarks = db.Bookmarks
                    .Where(b => b.UserId == userId)
                    .OrderByDescending(b => b.CreatedAt);
            }

            bookmarks.Select(x => x.User).Load();
            bookmarks.Select(x => x.Comments).Load();
            bookmarks.Select(x => x.BookmarkCategories).Load();
            bookmarks.Select(x => x.Upvotes).Load();

            foreach (var bk in bookmarks)
                foreach (var comm in bk.Comments)
                    db.Entry(comm).Reference(c => c.User).Load();


            ViewBag.currentUser = user;
            ViewBag.currentCategory = db.Categories.FirstOrDefault(c => c.Id == categoryId); ;
            ViewBag.currentBookmark = bookmarks.FirstOrDefault(b => b.Id == bookmarkId);
            ViewBag.categories = db.Categories.Where(c => c.UserId == userId);
            ViewBag.bookmarks = bookmarks.ToList();

            return View();
        }
    }
}
