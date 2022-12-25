﻿using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using project.net.Models;
using project.net.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace project.net.Controllers
{
    public class BookmarksController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<AppUser> userManager;

        public BookmarksController(
            ApplicationDbContext context, 
            IWebHostEnvironment host,
            UserManager<AppUser> userManager)
        {
            this.db = context;
            this.webHostEnvironment = host;
            this.userManager = userManager;
        }

        // Afisam pe pagina principala toate bookmarkurile in functie de popularitate
        [Route("")]
        public IActionResult Index([FromQuery(Name = "bookmarkId")] int? bookmarkId)
        {
            Bookmark bookmark = new();
            var allBookmarks = db.Bookmarks
                .Include("User")
                .Include("Comments")
                .Include("Comments.User")
                .Include("BookmarkCategories")
                .Include("Upvotes")
                .ToList();

            ViewBag.Bookmarks = allBookmarks;
            ViewBag.CurrentBookmark = allBookmarks.FirstOrDefault(b => b.Id == bookmarkId);

            var userId = userManager.GetUserId(User);
            ViewBag.MyCategories = db.Categories.Where(c => c.UserId == userId);

            return View(bookmark);
        }


        // Creem bookmarkul + optiuni

        [Authorize]
        [HttpPost]
        [Route("")]
        public IActionResult New(Bookmark bookmark)
        {
            var userId = userManager.GetUserId(User);
            ViewBag.MyCategories = db.Categories.Where(c => c.UserId == userId);

            ViewBag.Bookmarks = db.Bookmarks
                .Include("User")
                .Include("Comments")
                .Include("Comments.User")
                .Include("BookmarkCategories")
                .ToList();

            bookmark.UserId = userManager.GetUserId(User);
            bookmark.CreatedAt = DateTime.Now;

            if (!ModelState.IsValid)
                return View("Index", bookmark);

            var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
            var fileName = Guid.NewGuid() + "_" + bookmark.File?.FileName;
            var filePath = Path.Combine(uploadsFolder, fileName);
            bookmark.File?.CopyTo(new FileStream(filePath, FileMode.Create));
            bookmark.Image = fileName;

            db.Bookmarks.Add(bookmark);
            db.SaveChanges();
            
            TempData["message"] = "Bookmarkul a fost adaugat";
            return RedirectToAction("Index");
        }


        [Authorize]
        [HttpPost]
        [Route("/delete-bookmark/<bookmarkId:int>")]
        public IActionResult Delete(int bookmarkId)
        {   
             Bookmark bookmark = db.Bookmarks
                 .Include("Comments")
                 .First(b => b.Id == bookmarkId);

            if (userManager.GetUserId(User) != bookmark.UserId)
                return RedirectToAction("Index");

            db.Bookmarks.Remove(bookmark);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        [Route("/edit-bookmark")]
        public IActionResult Edit([FromBody]Bookmark bookmark)
        {
            var actualBookmark = db.Bookmarks.FirstOrDefault(b => b.Id == bookmark.Id);
            var userId = userManager.GetUserId(User);

            if (actualBookmark == null || actualBookmark?.UserId != userId)
                return NotFound();

            actualBookmark.Title = bookmark.Title;
            actualBookmark.Description = bookmark.Description;

            db.Bookmarks.Update(actualBookmark);
            db.SaveChanges();

            return new JsonResult(actualBookmark);
        }
    }
}
