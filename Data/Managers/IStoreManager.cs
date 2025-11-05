using Shared.Model;

namespace Data.Managers
{
    public interface IStoreManager
    {
        Task<List<Store>> GetStoresAsync();
    }
}