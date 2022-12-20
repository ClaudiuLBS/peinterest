using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace project.net.Controllers
{
    public class UpVotesController : Controller
    {
        [Authorize]
        [HttpPost]
        [Route("/upvote/<bookmarkId:int>/value")]
        public void New()
        {

        }
    }
}
