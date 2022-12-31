using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;


namespace project.net.Models
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public virtual ICollection<Bookmark>? Bookmarks { get; set; }

        public virtual ICollection<Category>? Categories { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; }
        
        public virtual ICollection<Upvote>? Upvotes { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? AllRoles { get; set; }
    }
}
