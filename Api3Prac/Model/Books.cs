using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api3Prac.Model
{
    public class Books
    {
        [Key]
        public int ID_Book { get; set; }
        public string Title { get; set; }


        public string Author { get; set; }
        public int YearOfPublic { get; set; }
        public string Description { get; set; }

        public int AvailableCopies { get; set; }

        [Required]
        [ForeignKey("BooksGenre")]
        public int ID_Genre { get; set; }
        
        public BooksGenre Genre { get; set; }

    }
}
