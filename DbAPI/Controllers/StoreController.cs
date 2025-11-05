using Data.Interfaces;
using Data.Managers;
using Microsoft.AspNetCore.Mvc;
using Shared.Model;

namespace DbAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreManager _repository;
        public StoreController(IStoreManager storeManager)
        {
            _repository = storeManager;
        }

        [HttpGet(Name = "GetStores")]
        public Task<List<Store>> GetStoresAsync()
        {
            return _repository.GetStoresAsync();

        }
    }
}
