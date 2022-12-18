using System.ComponentModel.DataAnnotations;
using project.net.Models;
namespace project.net.ViewModels
{
    public class CreateBookmark
    {
        [Required]
        public Bookmark? Bookmark { get; set; }

        [Required(ErrorMessage = "Imaginea e obligatorie")]
        public IFormFile? File { get; set; }
    }
}
