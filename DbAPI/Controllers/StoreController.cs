using Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Model;

namespace DbAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreRepository _repository;
        public StoreController(IStoreRepository storeManager)
        {
            _repository = storeManager;
        }

        [HttpGet("GetStores")]
        public async Task<List<Store>> GetStoresAsync()
        {
            return await _repository.GetStoresAsync();

        }
    }
}
