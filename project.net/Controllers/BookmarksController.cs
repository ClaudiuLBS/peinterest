using Microsoft.AspNetCore.Mvc;
using project.net.Models;
using project.net.Data;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using project.net.ViewModels;


namespace project.net.Controllers
{
    public class BookmarksController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;

        public BookmarksController(ApplicationDbContext context, IWebHostEnvironment host)
        {
            db = context;
            webHostEnvironment = host;
        }
        // Afisam pe pagina principala toate bookmarkurile in functie de popularitate
        [Route("")]
        public IActionResult Index([FromQuery(Name = "bookmarkId")] string? bookmarkId)
        {
            CreateBookmark createBookmark = new();
            if (bookmarkId != null)
                return Content(bookmarkId);
            else
                return View(createBookmark);
        }

       
        // Creem bookmarkul + optiuni

        [HttpPost]
        [Route("")]
        public IActionResult New(CreateBookmark createBookmark)
        {
            if (!ModelState.IsValid || createBookmark.File == null || createBookmark.Bookmark == null)
                return View("Index", createBookmark);

            createBookmark.Bookmark.CreatedAt = DateTime.Now;
            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
            string fileName = Guid.NewGuid() + "_" + createBookmark.File.FileName;
            string filePath = Path.Combine(uploadsFolder, fileName);
            createBookmark.File.CopyTo(new FileStream(filePath, FileMode.Create));
            createBookmark.Bookmark.Image = fileName;

            db.Bookmarks?.Add(createBookmark.Bookmark);
            db.SaveChanges();
            TempData["message"] = "Bookmarkul a fost adaugat";
            return RedirectToAction("Index");
        }
    }
}
