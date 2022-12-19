using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using project.net.Models;

namespace project.net.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) { }


        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<BookmarkCategory> BookmarkCategories { get; set; }
        public DbSet<Upvote> Upvotes { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BookmarkCategory>().HasKey(bc => new {
                bc.BookmarkId,
                bc.CategoryId
            });
            builder.Entity<BookmarkCategory>()
                .HasOne(bc => bc.Bookmark)
                .WithMany(b => b.BookmarkCategories)
                .HasForeignKey(bc => bc.BookmarkId);
            builder.Entity<BookmarkCategory>()
                .HasOne(bc => bc.Category)
                .WithMany(c => c.BookmarkCategories)
                .HasForeignKey(bc => bc.CategoryId);


            builder.Entity<Upvote>().HasKey(u => new {
                u.UserId,
                u.BookmarkId
            });
            builder.Entity<Upvote>()
                .HasOne(u => u.Bookmark)
                .WithMany(b => b.Upvotes)
                .HasForeignKey(u => u.BookmarkId);
            builder.Entity<Upvote>()
                .HasOne(u => u.User)
                .WithMany(u => u.Upvotes)
                .HasForeignKey(u => u.UserId);

        }
    }
}