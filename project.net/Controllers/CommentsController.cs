using System.Runtime.InteropServices.ComTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using project.net.Data;
using project.net.Models;

namespace project.net.Controllers
{
    public class CommentsController : Controller
    {

        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<AppUser> userManager;

        public CommentsController(
            ApplicationDbContext context,
            IWebHostEnvironment host,
            UserManager<AppUser> userManager)
        {
            this.db = context;
            this.webHostEnvironment = host;
            this.userManager = userManager;
        }


        [Authorize]
        [HttpPost]
        [Route("/post-comment/<bookmarkId:int>")]
        public IActionResult New(Bookmark bookmark, int bookmarkId)
        {
            if (bookmark.AddedComment?.Content == null)
                return RedirectToAction("Index", "Bookmarks", new { bookmarkId = bookmark.Id });

            Comment comment = new()
            {
                UserId = userManager.GetUserId(User),
                BookmarkId = bookmarkId,
                Content = bookmark.AddedComment.Content,
                Image = bookmark.AddedComment.Image,
                CreatedAt = DateTime.Now
            };
            db.Comments.Add(comment);
            db.SaveChanges();

            return RedirectToAction("Index", "Bookmarks", new { bookmarkId = bookmarkId });
        }
    }
}
