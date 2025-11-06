using Shared.Model;

namespace Data.Interfaces
{
    public interface IBookRepository
    {
        Task<bool> AddBookAsync(Book book);
        Task<bool> DeleteBookAsync(long ISBN);
        Task<bool> EditBookAsync(Book book);
        Task<List<Book>> GetBooksAsync();
    }
}