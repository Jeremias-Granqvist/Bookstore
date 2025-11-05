using Shared.Model;

namespace Bookstore.Service.Interfaces
{
    public interface IAuthorService
    {
        Task<List<Author>> GetAuthorsAsync();
        Task<Author> AddAuthorAsync(Author author);
        Task<Author> DeleteAuthorAsync(Author author);
        Task<bool> EditAuthorAsync(Author author);

    }
}