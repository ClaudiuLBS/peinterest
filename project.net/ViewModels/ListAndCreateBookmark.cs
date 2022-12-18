using project.net.Models;

namespace project.net.ViewModels
{
    public class ListAndCreateBookmark
    {
        public CreateBookmark? CreateBookmark { get; set; }
        public IEnumerable<Bookmark>? Bookmarks { get; set; }
    }
}
