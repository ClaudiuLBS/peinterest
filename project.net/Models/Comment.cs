using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace project.net.Models
{
    public class Comment
    {
        [Key]
        public int? Id { get; set; }

        public int? BookmarkId { get; set; }
        public virtual Bookmark? Bookmark { get; set; }

        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }

        [Required(ErrorMessage = "Continutul este obligatoriu")]
        public string? Content { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? Image { get; set; }

    }
}
