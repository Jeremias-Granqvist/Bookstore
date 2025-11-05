using Shared.Model;

namespace Bookstore.Service.Interfaces
{
    public interface IStoreService
    {
        Task<List<Store>> GetStoresAsync();
    }
}