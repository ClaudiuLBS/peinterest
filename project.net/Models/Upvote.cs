using System.ComponentModel.DataAnnotations;

namespace project.net.Models
{
    public class Upvote
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int BookmarkId { get; set; }
        public int Rating { get; set; }





    }
}
