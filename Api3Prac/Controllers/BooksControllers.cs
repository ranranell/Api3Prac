using Api3Prac.DbContextApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api3Prac.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Api3Prac.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private readonly TestApiDB _context;


        public BooksController(TestApiDB context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _context.Books.ToListAsync();
            return Ok(new
            {
                books = books,
                status = true
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound(new { message = "Книга не найдена" });
            }
            return Ok(new
            {
                book = book,
                status = true
            });
        }


        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] Books newBook)
        {
            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Книга успешно добавлена",
                status = true
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Books updatedBook)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound(new { message = "Книга не найдена" });
            }

            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.ID_Genre = updatedBook.ID_Genre;
            book.YearOfPublic = updatedBook.YearOfPublic;
            book.Description = updatedBook.Description;
            book.AvailableCopies = updatedBook.AvailableCopies;

            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Книга обнвлена",
                status = true
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound(new { message = "Книга не найдена" });
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Book deleted successfully",
                status = true
            });
        }

        [HttpGet("genre/{genreId}")]
        public async Task<IActionResult> GetBooksByGenre(int genreId)
        {
            var books = await _context.Books.Where(b => b.ID_Genre.ToString() == genreId.ToString()).ToListAsync();
            return Ok(new
            {
                books = books,
                status = true
            });
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks(string query)
        {
            var books = await _context.Books
                .Where(b => b.Title.Contains(query) || b.Author.Contains(query))
                .ToListAsync();

            return Ok(new
            {
                books = books,
                status = true
            });
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredBooks(
            [FromQuery] string title = null,
            [FromQuery] string author = null,
            [FromQuery] int? genreId = null,
            [FromQuery] int? yearOfPublic = null)
        {
            var booksQuery = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(title))
                booksQuery = booksQuery.Where(b => b.Title.Contains(title));

            if (!string.IsNullOrEmpty(author))
                booksQuery = booksQuery.Where(b => b.Author.Contains(author));

            if (genreId.HasValue)
                booksQuery = booksQuery.Where(b => b.ID_Genre == genreId.Value);

            if (yearOfPublic.HasValue)
                booksQuery = booksQuery.Where(b => b.YearOfPublic == yearOfPublic.Value);

            var books = await booksQuery.ToListAsync();

            return Ok(new
            {
                books = books,
                status = true
            });
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginatedBooks(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string title = null,
            [FromQuery] string author = null,
            [FromQuery] int? genreId = null,
            [FromQuery] int? yearOfPublic = null)
        {
            var booksQuery = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(title))
                booksQuery = booksQuery.Where(b => b.Title.Contains(title));

            if (!string.IsNullOrEmpty(author))
                booksQuery = booksQuery.Where(b => b.Author.Contains(author));

            if (genreId.HasValue)
                booksQuery = booksQuery.Where(b => b.ID_Genre == genreId.Value);

            if (yearOfPublic.HasValue)
                booksQuery = booksQuery.Where(b => b.YearOfPublic == yearOfPublic.Value);

            var totalItems = await booksQuery.CountAsync();
            var books = await booksQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                books = books,
                page = page,
                pageSize = pageSize,
                totalItems = totalItems,
                status = true
            });
        }
    }

}


