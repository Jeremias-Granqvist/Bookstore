using Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Model;

namespace DbAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookManager _repository;
        public BookController(IBookManager bookManager)
        {
            _repository = bookManager;
        }
        [HttpGet(Name = "GetBooks")]
        public Task<List<Book>> GetBooks()
        {
            return _repository.GetBooksAsync();

        }
        [HttpPost(Name = "CreateBook")]
        public async Task<ActionResult> CreateBookAsync(Book book)
        {
            var result = await _repository.AddBookAsync(book);
            if (!result) return Ok(book);
            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookAsync(int id)
        {
            var result = await _repository.DeleteBookAsync(id);
            if (!result) return NotFound();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(Book book, int id)
        {
            if (book == null)
            {
                return BadRequest("Book data is missing");
            }
            await _repository.EditBookAsync(book);
            return Ok();
        }
    }
}
