using project.net.Models;

namespace project.net.ViewModels
{
    public class BookmarksAndCategories
    {
        public IEnumerable<Bookmark>? Bookmarks { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
    }
}
