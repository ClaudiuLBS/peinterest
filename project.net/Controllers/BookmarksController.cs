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
        public IActionResult Index()
        {
            var bookmarks = db.Bookmarks;

            ViewBag.Bookmarks = bookmarks;
            return View();
        }

        // Afisam un singur articol in functie de id-ul sau
        // impreuna cu comentariile si numarul de upvote-uri

        public IActionResult Show(int id)
        {
            //afisam primul comm
            var bookmark = db.Bookmarks.Include("Comments")
                                        .Where(bm => bm.Id == id)
                                        .First();

            ViewBag.Bookmark = bookmark;
            ViewBag.Comments = bookmark.Comments;

            return View();
        }

        // Creem bookmarkul + optiuni

        public IActionResult New()
        {   

            return View();
        }

        public IActionResult (int id)




    }
}
