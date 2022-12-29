using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using project.net.Data;
using project.net.Models;

namespace project.net.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<AppUser> userManager;

        public CategoriesController(
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
        [Route("/add-category")]
        public IActionResult AddCategoryFromBookmark([FromBody]Category category)
        {
            var userId = userManager.GetUserId(User);
            category.UserId = userId;

            if (category.UserId == null  || category.Name == null)
                return NotFound();

            db.Categories.Add(category);
            db.SaveChanges();

            if (category.BookmarkId == null) 
                return new JsonResult(category);

            BookmarkCategory bookmarkCategory = new()
            {
                BookmarkId = category.BookmarkId,
                CategoryId = category.Id
            };
            db.BookmarkCategories.Add(bookmarkCategory);
            db.SaveChanges();

            return new JsonResult(new { id = category.Id, name = category.Name});
        }

        [Authorize]
        [HttpPost]
        [Route("/add-bookmark-to-category")]
        public IActionResult AddBookmarkToCategory([FromBody]BookmarkCategory bookmarkCategory)
        {
            if (bookmarkCategory.BookmarkId == null || bookmarkCategory.CategoryId == null)
                return NotFound(bookmarkCategory);
            
            var currentRelationship =
                db.BookmarkCategories
                    .Where(cr => cr.CategoryId == bookmarkCategory.CategoryId)
                    .FirstOrDefault(cr => cr.BookmarkId == bookmarkCategory.BookmarkId);

            if (currentRelationship == null)
            {
                db.BookmarkCategories.Add(bookmarkCategory);
                db.SaveChanges();
                return new JsonResult(new {action = "added"});
            }

            db.BookmarkCategories.Remove(currentRelationship);
            db.SaveChanges();
            return new JsonResult(new {action = "removed"});
        }

        [Authorize]
        [HttpPost]
        [Route("/edit-category")]
        public IActionResult Edit([FromBody] Category category)
        {
            var userId = userManager.GetUserId(User);
            if (userId == null)
                return NotFound();

            var actualCategory = db.Categories.FirstOrDefault(c => c.Id == category.Id);
            if (actualCategory == null || actualCategory.UserId != userId)
                return NotFound();

            actualCategory.Name = category.Name;
            db.Categories.Update(actualCategory);
            db.SaveChanges();

            return Json(new { categoryName = category.Name });
        }

        [Authorize]
        [HttpPost]
        [Route("/delete-category")]
        public IActionResult Delete([FromBody] Category category)
        {
            var userId = userManager.GetUserId(User);
            if (userId == null)
                return NotFound();

            var actualCategory = db.Categories.FirstOrDefault(c => c.Id == category.Id);
            if (actualCategory == null || actualCategory.UserId != userId)
                return NotFound();

            db.Categories.Remove(actualCategory);
            db.SaveChanges();

            return Json(new { deletedCategory = actualCategory.Id });
        }
    }
}
