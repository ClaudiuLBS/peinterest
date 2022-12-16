using System.ComponentModel.DataAnnotations;

namespace project.net.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public int BookmarkId { get; set; }
        public int UserId { get; set; }
        [Required(ErrorMessage = "Continutul este obligatoriu")]
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public string Image { get; set; }
        

    }
}
