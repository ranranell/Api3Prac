using Api3Prac.DbContextApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api3Prac.Model;

namespace Api3Prac.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        readonly TestApiDB _context;

        public UsersController(TestApiDB context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var Users = await _context.Users.ToListAsync();
            return Ok(new
            {
                Users = Users,
                status = true
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReaderById(int id)
        {
            var reader = await _context.Users.FindAsync(id);
            if (reader == null)
            {
                return NotFound(new { message = "Читатель не найден" });
            }
            return Ok(new
            {
                reader = reader,
                status = true
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddReader([FromBody] Users newReader)
        {
            _context.Users.Add(newReader);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Читатель добавлен",
                status = true
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReader(int id, [FromBody] Users updatedReader)
        {
            var reader = await _context.Users.FindAsync(id);
            if (reader == null)
            {
                return NotFound(new { message = "Читатель не найден" });
            }

            reader.UserName = updatedReader.UserName;
            reader.Surname = updatedReader.Surname;
            reader.Birthday = updatedReader.Birthday;
            reader.Email = updatedReader.Email;

            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Данные обнвлены ",
                status = true
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReader(int id)
        {
            var reader = await _context.Users.FindAsync(id);
            if (reader == null)
            {
                return NotFound(new { message = "Читатель не найден" });
            }

            _context.Users.Remove(reader);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Читатель удален",
                status = true
            });
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredReaders(
            [FromQuery] string userName = null,
            [FromQuery] string surname = null,
            [FromQuery] DateTime? birthday = null,
            [FromQuery] string email = null)
        {
            var readersQuery = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(userName))
                readersQuery = readersQuery.Where(u => u.UserName.Contains(userName));

            if (!string.IsNullOrEmpty(surname))
                readersQuery = readersQuery.Where(u => u.Surname.Contains(surname));

            if (birthday.HasValue)
                readersQuery = readersQuery.Where(u => u.Birthday == birthday.Value);

            if (!string.IsNullOrEmpty(email))
                readersQuery = readersQuery.Where(u => u.Email.Contains(email));

            var readers = await readersQuery.ToListAsync();

            return Ok(new
            {
                readers = readers,
                status = true
            });
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginatedReaders(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string userName = null,
            [FromQuery] string surname = null,
            [FromQuery] DateTime? birthday = null,
            [FromQuery] string email = null)
        {
            var readersQuery = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(userName))
                readersQuery = readersQuery.Where(u => u.UserName.Contains(userName));

            if (!string.IsNullOrEmpty(surname))
                readersQuery = readersQuery.Where(u => u.Surname.Contains(surname));

            if (birthday.HasValue)
                readersQuery = readersQuery.Where(u => u.Birthday == birthday.Value);

            if (!string.IsNullOrEmpty(email))
                readersQuery = readersQuery.Where(u => u.Email.Contains(email));

            var totalItems = await readersQuery.CountAsync();
            var readers = await readersQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                readers = readers,
                page = page,
                pageSize = pageSize,
                totalItems = totalItems,
                status = true
            });
        }
    }
}
