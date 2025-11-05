using Shared.Model;

namespace Bookstore.Service.Interfaces
{
    public interface IBookService
    {
        Task<Book> AddBookAsync(Book book);
        Task<Book> DeleteBookAsync(Book book);
        Task<bool> EditBookAsync(Book book);
        Task<List<Book>> GetBooksAsync();
    }
}