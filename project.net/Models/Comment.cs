using System.ComponentModel.DataAnnotations;

namespace project.net.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        [Required(ErrorMessage = "Continutul este obligatoriu")]
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int ArticleId { get; set; }
        public virtual Bookmark Bookmark { get; set; }

     // public virtual User user {get; set;}
     // image ?
    }
}
