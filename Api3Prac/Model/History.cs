using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api3Prac.Model
{
    public class History
    {
        [Key]
        public int HistoryId { get; set; }

        [Required]
        [ForeignKey("Books")]
        public int ID_book { get; set; }

        public Books Books { get; set; }

        [Required]
        [ForeignKey("Users")]
        public int ID_User{ get; set; }

        public Users Users { get; set; }

        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }

        public bool Status { get; set; }

    }
}
