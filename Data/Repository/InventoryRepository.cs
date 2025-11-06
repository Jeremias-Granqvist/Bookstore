using Shared.Model;
using Data.Context;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Statics;

namespace Data.Repository
{
    public class InventoryRepository : ModelBase, IInventoryRepository
    {
        private readonly BookstoreContext DBcontext;
        public InventoryRepository(BookstoreContext context)
        {
            DBcontext = context;
        }
        public async Task<List<Inventory>> GetInventoriesAsync()
        {
            return await DBcontext.Inventory
            .Include(i => i.InvBook)
            .Include(i => i.store)
            .Where(i => i.Amount > 0)
            .ToListAsync();
        }

        public async Task<bool> AddInventoryAsync(Inventory inventory)
        {
            using (DBcontext)
            {
                var existingInventory = DBcontext.Inventory.FirstOrDefault(i => i.InventoryISBN13 == inventory.InventoryISBN13 && i.StoreID == inventory.StoreID);
                if (existingInventory != null)
                {
                    existingInventory.Amount = inventory.Amount;
                }
                else
                {
                    DBcontext.Inventory.Add(inventory);
                }
                var response = await DBcontext.SaveChangesAsync();
                return StaticMethods.IsCrudOperationSuccessful(response);
            }
        }

        public async Task<bool> EditInventoryAsync(Inventory updatedInventory)
        {
            using (DBcontext)
            {
                var existingInventory = DBcontext.Inventory.FirstOrDefault(i => i.InventoryISBN13 == updatedInventory.InventoryISBN13 && i.StoreID == updatedInventory.StoreID);

                    existingInventory.Amount = updatedInventory.Amount;
                    var response = await DBcontext.SaveChangesAsync();
                    return StaticMethods.IsCrudOperationSuccessful(response);
            }
        }

        public async Task<bool> DeleteInventoryAsync(int StoreId, long ISBN)
        {
            using (DBcontext)
            {
                var inventoryToDelete = DBcontext.Inventory.FirstOrDefault(i => i.InventoryISBN13 == ISBN && i.StoreID == StoreId);
                DBcontext.Remove(inventoryToDelete.InventoryISBN13);
                var response = await DBcontext.SaveChangesAsync();
                return StaticMethods.IsCrudOperationSuccessful(response);
            }
        }
    }
}
