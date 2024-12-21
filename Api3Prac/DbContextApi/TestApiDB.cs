using Api3Prac.Model;
using Microsoft.EntityFrameworkCore;

namespace Api3Prac.DbContextApi
{
    public class TestApiDB : DbContext
    {
        public TestApiDB(DbContextOptions options) : base(options){ }
        public DbSet<Users> Users { get; set; }

        public DbSet<Books> Books { get; set; }
        public DbSet<BooksGenre> BooksGenre { get; set; }
        public DbSet<History> History { get; set; }
    }
}
