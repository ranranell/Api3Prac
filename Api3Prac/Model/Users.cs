using System.ComponentModel.DataAnnotations;

namespace Api3Prac.Model
{
    public class Users
    {
        [Key]
        public int ID_User { get; set; }

        public string UserName { get; set; }

        public string Surname { get; set; }
        public DateTime Birthday { get; set; }
        public string Email { get; set; }
    }
}
