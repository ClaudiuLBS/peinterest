using System.ComponentModel.DataAnnotations.Schema;

namespace project.net.Models
{
    public class BookmarkCategory
    {
        public int? BookmarkId { get; set; }
        [ForeignKey("BookmarkId")]
        public virtual Bookmark? Bookmark { get; set; }

        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
    }
}
