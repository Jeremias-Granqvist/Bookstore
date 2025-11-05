using Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Model;

namespace DbAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
     private readonly IInventoryManager _repository;
    public InventoryController(IInventoryManager inventoryManager)
    {
        _repository = inventoryManager;
    }
    [HttpGet(Name = "GetInventories")]
    public Task<List<Inventory>> GetInventories()
    {
        return _repository.GetInventoriesAsync();

    }
    [HttpPost(Name = "CreateInventory")]
    public async Task<ActionResult> CreateInventoryAsync(Inventory inventory)
    {
        var result = await _repository.AddInventoryAsync(inventory);
        if (!result) return Ok(inventory);
        return NotFound();
    }

    [HttpDelete("{StoreId}/{ISBN}")]
    public async Task<IActionResult> DeleteInventoryAsync(int StoreId, long ISBN)
    {
        var result = await _repository.DeleteInventoryAsync(StoreId, ISBN);
        if (!result) return NotFound();
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInventory(Inventory inventory, int id)
    {
        if (inventory == null)
        {
            return BadRequest("Inventory data is missing");
        }
        var inventoryToUpdate = await _repository.EditInventoryAsync(inventory);
        return Ok();
    }
}
}
