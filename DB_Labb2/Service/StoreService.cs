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
    public class StoreService : IStoreService
    {
        private readonly HttpClient _httpClient;
        private readonly IEventDispatcher _eventDispatcher;
        public StoreService(IHttpClientFactory httpClient, IEventDispatcher eventDispatcher)
        {
            _httpClient = httpClient.CreateClient("api");
            _httpClient.BaseAddress = new Uri(StaticConstants.API_BASE_ADRESS);
            _eventDispatcher = eventDispatcher;
        }
        public async Task<List<Store>> GetStoresAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Store>>("api/Store/GetStores");
            if (response is null) return new List<Store>();
            return response;
        }
    }
}
