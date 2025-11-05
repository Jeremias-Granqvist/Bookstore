using Shared.Model;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IAuthorManager
    {
        Task<bool> AddAuthorAsync(Author author);
        Task<bool> DeleteAuthorAync(int authorID);
        Task<bool> EditAuthorAsync(Author updatedAuthor);
        Task<List<Author>> GetAuthorsAsync();
    }
}