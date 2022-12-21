using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult New(Upvote upvote)
        {
            Upvote? currentUpvote =
                db.Upvotes
                    .Where(uv => uv.UserId == upvote.UserId)
                    .Where(uv => uv.BookmarkId == upvote.BookmarkId)
                    .FirstOrDefault();

            if (currentUpvote != null && currentUpvote.Rating == upvote.Rating)
                upvote.Rating = 0;

            db.Upvotes.Add(upvote);
            db.SaveChanges();

            return new JsonResult(upvote);
        }
    }
}
