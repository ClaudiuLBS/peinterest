using project.net.Data;
using project.net.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace project.net.Controllers
{
    [Authorize(Roles = "Admin")]
    //[Route("/admin")]
    public class AdminPanelController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<AppUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;


        public AdminPanelController(
               ApplicationDbContext context,
               UserManager<AppUser> userManager,
               RoleManager<IdentityRole> roleManager
               )
        {
            db = context;

            _userManager = userManager;

            _roleManager = roleManager;
        }
        [Route("/admin")]
        public IActionResult Index()
        {
            var users = from user in db.Users
                        orderby user.UserName
                        select user;

            ViewBag.UsersList = users;

            return View();
        }
        [Route("/admin/show-user/{userId}")]
        public async Task<ActionResult> Show(string userId)
        {
            AppUser user = db.Users.Find(userId);
            var roles = await _userManager.GetRolesAsync(user);

            ViewBag.Roles = roles;

            return View(user);
        }

        [Route("admin/edit-user/{userId}")]
        public async Task<ActionResult> Edit(string userId)
        {
            AppUser user = db.Users.Find(userId);

            user.AllRoles = GetAllRoles();

            var roleNames = await _userManager.GetRolesAsync(user); // Lista de nume de roluri

            // Cautam ID-ul rolului in baza de date
            var currentUserRole = _roleManager.Roles
                .FirstOrDefault(r => roleNames.Contains(r.Name)); // Selectam 1 singur rol
            ViewBag.UserRole = currentUserRole != null ? currentUserRole.Id : "None";
            return View(user);
        }

        [HttpPost]
        [Route("admin/edit-user/{id}")]
        public async Task<ActionResult> Edit(string id, AppUser newData, [FromForm] string newRole)
        {
            AppUser user = db.Users.Find(id);

            user.AllRoles = GetAllRoles();


            if (ModelState.IsValid)
            {
                user.UserName = newData.UserName;
                user.Email = newData.Email;
                user.FirstName = newData.FirstName;
                user.LastName = newData.LastName;
                user.PhoneNumber = newData.PhoneNumber;


                // Cautam toate rolurile din baza de date
                var roles = db.Roles.ToList();

                foreach (var role in roles)
                {
                    // Scoatem userul din rolurile anterioare
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                // Adaugam noul rol selectat
                var roleName = await _roleManager.FindByIdAsync(newRole);
                await _userManager.AddToRoleAsync(user, roleName.ToString());

                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("admin/delete-user/{id}")]
        public IActionResult Delete(string id)
        {
            var user = db.Users
                         .Include("Bookmarks")
                         .Include("Categories")
                         .Include("Comments")
                         .Include("Upvotes")
                         .Include("Bookmarks.BookmarkCategories")
                         .Include("Bookmarks.Comments")
                         .Include("Bookmarks.Upvotes")
                         .FirstOrDefault(u => u.Id == id);

            if (user == null) return NotFound();


            // Delete user saved bookmarks
            if (user.Categories != null)
                foreach (Category category in user.Categories)
                {
                    if (category.BookmarkCategories != null && category.BookmarkCategories.Count > 0)
                        foreach (BookmarkCategory bc in category.BookmarkCategories)
                            db.BookmarkCategories.Remove(bc);
                    db.Categories.Remove(category);
                }


            // Delete user comments
            if (user.Comments != null)
                foreach (var comment in user.Comments)
                    db.Comments.Remove(comment);

            // Delete user upvotes 
            if (user.Upvotes != null)
                foreach (var upvote in user.Upvotes)
                    db.Upvotes.Remove(upvote);

            // Delete user bookmarks
            if (user.Bookmarks != null)
                foreach (var bookmark in user.Bookmarks)
                {
                    if (bookmark.Comments != null)
                        foreach (var comment in bookmark.Comments)
                            db.Comments.Remove(comment);

                    if (bookmark.Upvotes != null)
                        foreach (var upvote in bookmark.Upvotes)
                            db.Upvotes.Remove(upvote);

                    if (bookmark.BookmarkCategories != null)
                        foreach (var bc in bookmark.BookmarkCategories)
                            db.BookmarkCategories.Remove(bc);

                    db.Bookmarks.Remove(bookmark);
                }


            db.AppUsers.Remove(user);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        [NonAction]
        public IEnumerable<SelectListItem> GetAllRoles()
        {
            var selectList = new List<SelectListItem>();

            var roles = from role in db.Roles
                        select role;

            foreach (var role in roles)
            {
                selectList.Add(new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name.ToString()
                });
            }
            return selectList;
        }
    }
}