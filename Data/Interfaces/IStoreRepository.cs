using Shared.Model;

namespace Data.Interfaces
{
    public interface IStoreRepository
    {
        Task<List<Store>> GetStoresAsync();
    }
}