using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.net.Data;
using project.net.Models;

namespace project.net.Controllers
{
    public class UpVotesController : Controller
    {

        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<AppUser> userManager;

        public UpVotesController(
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
        [Route("/upvote")]
        public IActionResult New([FromBody]Upvote upvote)
        {
            Upvote? currentUpvote =
                db.Upvotes
                    .Where(uv => uv.UserId == upvote.UserId)
                    .Where(uv => uv.BookmarkId == upvote.BookmarkId)
                    .FirstOrDefault();

            if (currentUpvote != null)
            {
                currentUpvote.Rating = currentUpvote.Rating == upvote.Rating ? 0 : upvote.Rating;
                db.Upvotes.Update(currentUpvote);
                db.SaveChanges();
                return new JsonResult(currentUpvote);
            }

            db.Upvotes.Add(upvote);
            db.SaveChanges();

            var bookmark = db.Bookmarks.Include("Upvote").FirstOrDefault(b => b.Id == upvote.BookmarkId);
            var likes = bookmark.Upvotes!.Count(uv => uv.Rating == 1);
            var dislikes = bookmark.Upvotes!.Count(uv => uv.Rating == -1);

            return new JsonResult(new {upvote, rating = likes - dislikes });
        }
    }
}
