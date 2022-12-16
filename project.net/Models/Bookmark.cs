using System.ComponentModel.DataAnnotations;
namespace project.net.Models

{
    public class Bookmark
    {
        [Key]
        public int Id { get; set; }
        //[Key] ?
        public int userId { get; set; }
 
        [Required(ErrorMessage = "Titlul este obligatoriu")]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Categoria este obligatorie")]

        public int upVotes { get; set; }
        //image


    }
}
