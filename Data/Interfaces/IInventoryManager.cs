using Shared.Model;

namespace Data.Interfaces
{
    public interface IInventoryManager
    {
        Task<bool> AddInventoryAsync(Inventory inventory);
        Task<bool> DeleteInventoryAsync(int StoreId, long ISBN);
        Task<bool> EditInventoryAsync(Inventory updatedInventory);
        Task<List<Inventory>> GetInventoriesAsync();
    }
}