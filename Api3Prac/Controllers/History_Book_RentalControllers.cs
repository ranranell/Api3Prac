using Api3Prac.DbContextApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api3Prac.Model;

namespace Api3Prac.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoryControllers : Controller
    {
        readonly TestApiDB _context;

        public HistoryControllers(TestApiDB context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> RentBook([FromBody] History newRental)
        {
            _context.History.Add(newRental);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Книга арендована ",
                status = true
            });
        }

        [HttpPut("return/{id}")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            var rental = await _context.History.FindAsync(id);
            if (rental == null)
            {
                return NotFound(new { message = "Запись об аренде не найдена" });
            }

            rental.Status = false;
            rental.ReturnDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Книга возвращена",
                status = true
            });
        }

        [HttpGet("history/reader/{readerId}")]
        public async Task<IActionResult> GetRentalHistoryForReader(int readerId)
        {
            var rentals = await _context.History
                .Where(r => r.ID_User == readerId)
                .ToListAsync();

            return Ok(new
            {
                rentals = rentals,
                status = true
            });
        }

        [HttpGet("history/book/{bookId}")]
        public async Task<IActionResult> GetRentalHistoryForBook(int bookId)
        {
            var rentals = await _context.History
                .Where(r => r.ID_book == bookId)
                .ToListAsync();

            return Ok(new
            {
                rentals = rentals,
                status = true
            });
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentRentals()
        {
            var rentals = await _context.History
                .Where(r => r.Status == true)
                .ToListAsync();

            return Ok(new
            {
                rentals = rentals,
                status = true
            });
        }
    }
}
