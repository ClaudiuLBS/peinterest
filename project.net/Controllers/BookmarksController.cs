using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using project.net.Models;
using project.net.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace project.net.Controllers
{
    public class BookmarksController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<AppUser> userManager;

        public BookmarksController(
            ApplicationDbContext context, 
            IWebHostEnvironment host,
            UserManager<AppUser> userManager)
        {
            this.db = context;
            this.webHostEnvironment = host;
            this.userManager = userManager;
        }

        // Afisam pe pagina principala toate bookmarkurile in functie de popularitate
        [Route("")]
        public IActionResult Index()
        {
            Bookmark bookmark = new();
            ViewBag.Bookmarks = db.Bookmarks
                .Include("User")
                .Include("Comments")
                .Include("Comments.User")
                .ToList();

            return View(bookmark);
        }


        // Creem bookmarkul + optiuni

        [Authorize]
        [HttpPost]
        [Route("")]
        public IActionResult New(Bookmark bookmark)
        {
            ViewBag.Bookmarks = db.Bookmarks
                .Include("User")
                .Include("Comments")
                .Include("Comments.User")
                .ToList();

            bookmark.UserId = userManager.GetUserId(User);
            bookmark.CreatedAt = DateTime.Now;

            if (!ModelState.IsValid)
                return View("Index", bookmark);

            var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
            var fileName = Guid.NewGuid() + "_" + bookmark.File?.FileName;
            var filePath = Path.Combine(uploadsFolder, fileName);
            bookmark.File?.CopyTo(new FileStream(filePath, FileMode.Create));
            bookmark.Image = fileName;

            db.Bookmarks.Add(bookmark);
            db.SaveChanges();
            
            TempData["message"] = "Bookmarkul a fost adaugat";
            return RedirectToAction("Index");
        }


        [Authorize]
        [HttpPost]
        [Route("/delete-bookmark/<bookmarkId:int>")]
        public IActionResult Delete(int bookmarkId)
        {   
             Bookmark bookmark = db.Bookmarks
                 .Include("Comments")
                 .First(b => b.Id == bookmarkId);

            if (userManager.GetUserId(User) != bookmark.UserId)
                return RedirectToAction("Index");

            db.Bookmarks.Remove(bookmark);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
