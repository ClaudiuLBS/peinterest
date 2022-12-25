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
            var userId = userManager.GetUserId(User);

            //cautam votul curent al userului
            var currentUpvote = db.Upvotes
                .Where(uv => uv.UserId == userId)
                .FirstOrDefault(uv => uv.BookmarkId == upvote.BookmarkId);

            //verificam daca userul a mai votat bookmarkul
            if (currentUpvote != null)
            {
                //actualizam ratingul
                currentUpvote.Rating = currentUpvote.Rating == upvote.Rating ? 0 : upvote.Rating;
                db.Upvotes.Update(currentUpvote);
                db.SaveChanges();
            }
            else
            {
                //adaugam unul nou
                currentUpvote = upvote;
                db.Upvotes.Add(currentUpvote);
                db.SaveChanges();
            }

            //calculam noua dinferenta dintre like-uri si dislike-uri
            var bookmark = db.Bookmarks.Include("Upvotes").FirstOrDefault(b => b.Id == currentUpvote.BookmarkId);
            var likes = bookmark.Upvotes.Count(uv => uv.Rating == 1);
            var dislikes = bookmark.Upvotes.Count(uv => uv.Rating == -1);

            return new JsonResult(new {userRating = currentUpvote.Rating, rating = likes - dislikes });
        }
    }
}
