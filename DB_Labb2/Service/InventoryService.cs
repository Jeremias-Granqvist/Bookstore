using Bookstore.Service.Interfaces;
using Shared.Model;
using Shared.Statics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Service
{
    public class InventoryService : IInventoryService
    {
        private readonly HttpClient _httpClient;
        private readonly IEventDispatcher _eventDispatcher;
        public InventoryService(IHttpClientFactory httpClient, IEventDispatcher eventDispatcher)
        {
            _httpClient = httpClient.CreateClient("api");
            _httpClient.BaseAddress = new Uri(StaticConstants.API_BASE_ADRESS);
            _eventDispatcher = eventDispatcher;
        }
        public async Task<List<Inventory>> GetInventoriesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Inventory>>("api/Inventory/GetInventories");
            if (response is null) return new List<Inventory>();
            return response;
        }
        public async Task<Inventory> AddInventoryAsync(Inventory inventory)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync<Inventory>("api/CreateInventory", inventory);
                if (response.IsSuccessStatusCode) _eventDispatcher.EntityAdded(inventory);
                return inventory;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}, {ex.InnerException}");
                throw;
            }
        }
        public async Task<Inventory> DeleteInventoryAsync(Inventory inventory)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/DeleteBook/{inventory.StoreID}/{inventory.InventoryISBN13}");
                if (response.IsSuccessStatusCode) _eventDispatcher.EntityRemoved(inventory);
                return inventory;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}, {ex.InnerException}");
                throw;
            }
        }
        public async Task<bool> EditInventoryAsync(Inventory inventory)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/UpdateInventory/{inventory.StoreID}", inventory);
                if (response.IsSuccessStatusCode) _eventDispatcher.EntityUpdated(inventory);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}, {ex.InnerException}");
                throw;
            }
        }

    }
}
