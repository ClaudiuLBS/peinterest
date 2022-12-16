using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.net.Models
{
    public class Upvote
    {
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual AppUser? User { get; set; }

        public int? BookmarkId { get; set; }
        [ForeignKey("BookmarkId")]
        public virtual Bookmark? Bookmark { get; set; }

        public int? Rating { get; set; }
    }
}
