using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using project.net.Models;

namespace project.net.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BookmarkCategory>().HasKey(bc => new
            {
                bc.BookmarkId,
                bc.CategoryId
            });

            builder.Entity<BookmarkCategory>().HasOne(bc => bc.Bookmark).WithMany(bc => bc.BookmarkCategories)
                .HasForeignKey(bc => bc.BookmarkId);
            builder.Entity<BookmarkCategory>().HasOne(bc => bc.Category).WithMany(bc => bc.BookmarkCategories)
                .HasForeignKey(bc => bc.CategoryId);
            base.OnModelCreating(builder);
        }

        public DbSet<Bookmark>? Bookmarks { get; set; }

        public DbSet<Category>? Categories { get; set; }

        public DbSet<Comment>? Comments { get; set; }

        public DbSet<BookmarkCategory>? BookmarkCategories { get; set; }

        public DbSet<Upvote>? Upvotes { get; set; }

        public DbSet<AppUser>? AppUsers { get; set; }
    }
}