using Shared.Model;

namespace Data.Interfaces
{
    public interface IBookManager
    {
        Task<bool> AddBookAsync(Book book);
        Task<bool> DeleteBookAsync(long ISBN);
        Task<bool> EditBookAsync(Book book);
        Task<List<Book>> GetBooksAsync();
    }
}