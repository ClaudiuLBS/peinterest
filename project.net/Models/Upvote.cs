using System.ComponentModel.DataAnnotations;

namespace project.net.Models
{
    public class Upvote
    {
        [Key]
        public int UpvoteId { get; set; }

        public int Rating { get; set; }

        public virtual Bookmark Bookmark { get; set; }

        //public virtual User User{get; set;}


    }
}
