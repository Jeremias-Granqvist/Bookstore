using Shared.Model;

namespace Bookstore.Service.Interfaces
{
    public interface IInventoryService
    {
        Task<Inventory> AddInventoryAsync(Inventory inventory);
        Task<bool> EditInventoryAsync(Inventory inventory);
        Task<List<Inventory>> GetInventoriesAsync();
        Task<Inventory> DeleteInventoryAsync(Inventory inventory);
    }
}