using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.net.Models

{
    public class Bookmark
    {
        [Key]
        public int? Id { get; set; }

        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }
     
        [Required(ErrorMessage = "Titlul este obligatoriu")]
        [StringLength(100, ErrorMessage = "Titlul nu poate avea mai mult de 100 de caractere")]
        [MinLength(3, ErrorMessage = "Titlul trebuie sa aiba mai mult de 3 caractere")]
        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string? Image { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; }  

        public virtual ICollection<BookmarkCategory>? BookmarkCategories { get; set; }

        public virtual ICollection<Upvote>? Upvotes { get; set; }

        [Required(ErrorMessage = "Imaginea e obligatorie")]
        [NotMapped]
        public IFormFile? File { get; set; }

        [NotMapped]
        public Comment AddedComment { get; set; }

    }
}
