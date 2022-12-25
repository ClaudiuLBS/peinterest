using project.net.Models;

namespace project.net.ViewModels
{
    public class BookmarksAndCategories
    {
        public Bookmark? Bookmark { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
    }
}
