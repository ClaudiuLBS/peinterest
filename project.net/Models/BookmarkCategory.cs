using System.ComponentModel.DataAnnotations.Schema;

namespace project.net.Models
{
    public class BookmarkCategory
    {
        public int? BookmarkId { get; set; }
        public virtual Bookmark? Bookmark { get; set; }

        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}
