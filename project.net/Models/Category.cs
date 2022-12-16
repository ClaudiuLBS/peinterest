using System.ComponentModel.DataAnnotations;

namespace project.net.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; } 
        [Required(ErrorMessage = "Numele categoriei este obligatoriu")]
        public string Name { get; set; }
        public virtual ICollection<Bookmark> Bookmarks{ get; set; }


    }
}
