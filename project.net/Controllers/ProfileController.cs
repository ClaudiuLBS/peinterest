using Microsoft.AspNetCore.Mvc;

namespace project.net.Controllers
{
    public class ProfileController : Controller
    {
        [Route("profile/{id}")]
        public IActionResult Index(string id) 
        {
            return Content($"User {id}");
        }

        [Route("profile/{id}/categories")]
        public IActionResult UserBookmarks(string id)
        {
            return Content($"Bookmarks of user {id}");
        }
    }
}
