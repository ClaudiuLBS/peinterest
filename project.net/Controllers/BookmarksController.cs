﻿using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using project.net.Models;
using project.net.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Internal;
using System.Data;
using Ganss.Xss;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Linq;


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
            //var allBookmarks = db.Bookmarks
            //    .Include("User")
            //    .Include("Comments")
            //    .Include("Comments.User")
            //    .Include("BookmarkCategories")
            //    .Include("Upvotes")
            //    .ToList();

            var allBookmarks = db.Bookmarks;
            allBookmarks.Select(x => x.User).Load();
            allBookmarks.Select(x => x.Comments).Load();
            allBookmarks.Select(x => x.BookmarkCategories).Load();
            allBookmarks.Select(x => x.Upvotes).Load();

            foreach (var item in allBookmarks)
                if (item.Comments != null)
                    foreach (var comm in item.Comments)
                        db.Entry(comm).Reference(c => c.User).Load();

            ViewBag.Bookmarks = allBookmarks.ToList();
            ViewBag.CurrentBookmark = allBookmarks.FirstOrDefault(b => b.Id == bookmarkId);

            var userId = userManager.GetUserId(User);
            ViewBag.MyCategories = db.Categories.Where(c => c.UserId == userId);

            //Motor de cautare

            var search = "";
            /*
            if (Convert.ToString(HttpContext.Request.Query["search"]) != null)
            {
                search = Convert.ToString(HttpContext.Request.Query["search"]).Trim(); // eliminam spatiile libere 

                List<int?> bookmarksIds = db.Bookmarks.Where
                                        (
                                         b => b.Title.Contains(search)
                                         || b.Description.Contains(search)
                                        ).Select(a => a.Id).ToList();
                allBookmarks = db.Bookmarks.Where(b => bookmarksIds.Contains(b.Id))
                                            .Include("User")
                                            .Include("Comments")
                                            .Include("Comments.User")
                                            .Include("BookmarkCategories")
                                            .Include("Upvotes")
                                            .ToList();

            }
            */

            ViewBag.SearchString = search;

            // Afisare paginata
            // afisam 20 de postari pe pagina
            int _perPage = 3;

            if(TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            //verificam de fiecare data cu count() cate postari sunt la momentul dat

            int totalPosts = allBookmarks.Count();

            // Se preia pagina curenta din View-ul asociat
            // Numarul paginii este valoarea parametrului page din ruta

            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);

            var offset = 0;

            // Se calculeaza offsetul in functie de numarul paginii la care suntem
            if (!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * _perPage;
            }
            var paginatedBookmarks = allBookmarks.Skip(offset).Take(_perPage);

            // Preluam numarul ultimei pagini
            ViewBag.lastPage = Math.Ceiling((float)totalPosts / (float)_perPage);

            // Trimitem articolele cu ajutorul unui ViewBag catre View-ul corespunzator
            ViewBag.Bookmarks = paginatedBookmarks;


            if(search != "")
            {
                ViewBag.PaginationBaseUrl = "/?search=" + search + "&page";
            }
            else
            {
                ViewBag.PaginationBaseUrl = "/?page";
            }

            return View();
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


        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [Route("/delete-bookmark/<bookmarkId:int>")]
        public IActionResult Delete(int bookmarkId)
        {
            Bookmark bookmark = db.Bookmarks
                     .Include("Comments")
                     .Include("Upvotes")
                     .Include("BookmarkCategories")
                     .First(b => b.Id == bookmarkId);

            // Delete bookmark comments
            if (bookmark.Comments.Count > 0)
            {
                foreach (var comment in bookmark.Comments)
                {
                    db.Comments.Remove(comment);
                }
            }
            // Delete bookmark upvotes
            if (bookmark.Upvotes.Count > 0)
            {
                foreach (var upvote in bookmark.Upvotes)
                {
                    db.Upvotes.Remove(upvote);
                }
            }
            // Delete bookmark from users saved bookmarks
            if (bookmark.BookmarkCategories.Count > 0)
            {
                foreach (var savedbookmark in bookmark.BookmarkCategories)
                {
                    db.BookmarkCategories.Remove(savedbookmark);
                }
            }


            if (userManager.GetUserId(User) != bookmark.UserId && !User.IsInRole("Admin"))
                return RedirectToAction("Index");

            db.Bookmarks.Remove(bookmark);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [Route("/edit-bookmark")]
        public IActionResult Edit([FromBody]Bookmark bookmark)
        {
            var actualBookmark = db.Bookmarks.FirstOrDefault(b => b.Id == bookmark.Id);
            var userId = userManager.GetUserId(User);

            if (actualBookmark == null || (actualBookmark?.UserId != userId && !User.IsInRole("Admin")))
                return NotFound();

            actualBookmark.Title = bookmark.Title;
            actualBookmark.Description = bookmark.Description;

            db.Bookmarks.Update(actualBookmark);
            db.SaveChanges();

            return new JsonResult(actualBookmark);
        }
     

    }

}
