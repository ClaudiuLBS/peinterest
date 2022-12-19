using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.net.Models
{
    public class Category
    {
        [Key]
        public int? Id { get; set; }

        public string? UserId { get; set; } 
        public virtual AppUser? User { get; set; }

        [Required(ErrorMessage = "Numele categoriei este obligatoriu")]
        public string? Name { get; set; }

        public virtual ICollection<BookmarkCategory>? BookmarkCategories { get; set; }
    }
}
