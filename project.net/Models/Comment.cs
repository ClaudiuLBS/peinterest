using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.net.Models
{
    public class Comment
    {
        [Key]
        public int? Id { get; set; }

        public int? BookmarkId { get; set; }
        [ForeignKey("BookmarkId")]
        public virtual Bookmark? Bookmark { get; set; }

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual AppUser? User { get; set; }

        [Required(ErrorMessage = "Continutul este obligatoriu")]
        public string? Content { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? Image { get; set; }

    }
}
