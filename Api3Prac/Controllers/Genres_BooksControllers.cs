using Api3Prac.DbContextApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api3Prac.Model;

namespace Api3Prac.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : Controller
    {
        private readonly TestApiDB _context;

        public GenresController(TestApiDB context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGenres()
        {
            var genres = await _context.BooksGenre.ToListAsync();
            return Ok(new
            {
                genres = genres,
                status = true
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddGenre([FromBody] BooksGenre newGenre)
        {
            _context.BooksGenre.Add(newGenre);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Жанр добавлен",
                status = true
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenre(int id, [FromBody] BooksGenre updatedGenre)
        {
            var genre = await _context.BooksGenre.FindAsync(id);
            if (genre == null)
            {
                return NotFound(new { message = "Жанр не найден" });
            }

            genre.Name = updatedGenre.Name;
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Жанр обнвлен",
                status = true
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var genre = await _context.BooksGenre.FindAsync(id);
            if (genre == null)
            {
                return NotFound(new { message = "Жанр не найден" });
            }

            _context.BooksGenre.Remove(genre);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Жанр удален",
                status = true
            });
        }
    }

}
