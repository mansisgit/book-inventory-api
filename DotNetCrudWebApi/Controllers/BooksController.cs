using DotNetCrudWebApi.Data;
using DotNetCrudWebApi.Books;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetCrudWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public BooksController(AppDbContext AppDbContext)
        {
            _appDbContext = AppDbContext;
        }

        // Get : api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookModel>>> GetBooks()
        {
            if (_appDbContext.Books == null)
            {
                return NotFound();
            }
            return await _appDbContext.Books.ToListAsync();
        }

        // Get : api/Books/2
        [HttpGet("{id}")]
        public async Task<ActionResult<BookModel>> GetBook(int id)
        {
            if (_appDbContext.Books is null)
            {
                return NotFound();
            }
            var book = await _appDbContext.Books.FindAsync(id);
            if (book is null)
            {
                return NotFound();
            }
            return book;
        }

        // Post : api/Books
        [HttpPost]
        public async Task<ActionResult<BookModel>> PostBook(BookModel book)
        {
            _appDbContext.Books.Add(book);
            await _appDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        // Put : api/Books/2
        [HttpPut]
        public async Task<ActionResult<BookModel>> PutBook(int id, BookModel book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }
            _appDbContext.Entry(book).State = EntityState.Modified;
            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        private bool BookExists(long id)
        {
            return (_appDbContext.Books?.Any(book => book.Id == id)).GetValueOrDefault();
        }

        // Delete : api/Books/2
        [HttpDelete("{id}")]
        public async Task<ActionResult<BookModel>> DeleteBook(int id)
        {
            if (_appDbContext.Books is null)
            {
                return NotFound();
            }
            var book = await _appDbContext.Books.FindAsync(id);
            if (book is null)
            {
                return NotFound();
            }
            _appDbContext.Books.Remove(book);
            await _appDbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}