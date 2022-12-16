using System.ComponentModel.DataAnnotations;

namespace project.net.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Numele categoriei este obligatoriu")]
        public string CategoryName { get; set; }
        public virtual ICollection<Bookmark> Bookmarks{ get; set; }
 //     public virtual User User { get; set; }

    }
}
