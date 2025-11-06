using Microsoft.AspNetCore.Mvc;
using Shared.Model;
using Data.Interfaces;
using System.Net.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DbAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _repository ;
        public AuthorController(IAuthorRepository authorManager)
        {
            _repository = authorManager;
        }
        [HttpGet("GetAuthors")]
        public Task<List<Author>> GetAuthors()
        {
            return _repository.GetAuthorsAsync();

        }
        [HttpPost(Name = "CreateAuthor")]
        public async Task<ActionResult> CreateAuthorAsync(Author author)
        {
            var result = await _repository.AddAuthorAsync(author);
            if(!result) return Ok(author);
            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthorAsync(int id)
        {
            var result = await _repository.DeleteAuthorAync(id);
            if (!result) return NotFound();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(Author author, int id)
        {
            if (author == null)
            {
                return BadRequest("Author data is missing");
            }
            var authorToUpdate = await _repository.EditAuthorAsync(author);
            return Ok();
        }
    }
}
