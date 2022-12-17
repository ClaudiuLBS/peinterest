using Microsoft.AspNetCore.Mvc;
using project.net.Models;
using project.net.Data;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace project.net.Controllers
{
    public class BookmarksController : Controller
    {
        private readonly ApplicationDbContext db;

        public BookmarksController(ApplicationDbContext context)
        {
            db = context;
        }
        // Afisam pe pagina principala toate bookmarkurile in functie de popularitate
        [Route("")]
        public IActionResult Index([FromQuery(Name = "bookmarkId")] string? bookmarkId)
        {
            Bookmark bookmark = new();
            if (bookmarkId != null)
                return Content(bookmarkId);
            else
                return View(bookmark);
        }

       
        // Creem bookmarkul + optiuni

        [HttpPost]
        [Route("/new-bookmark")]
        public IActionResult New(Bookmark bookmark)
        {
            if (ModelState.IsValid)
            {
                bookmark.CreatedAt = DateTime.Now;
                db.Bookmarks.Add(bookmark);
                db.SaveChanges();
                TempData["message"] = "Bookmarkul a fost adaugat";
                return RedirectToAction("Index");
            }
            return View("Index", bookmark);
        }
        


    }
}
