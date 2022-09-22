using Microsoft.AspNetCore.Mvc;
using SpeedokuRoyaleServer.Models;
using SpeedokuRoyaleServer.Models.Services.MariaDB;

namespace SpeedokuRoyaleServer.Controllers;

[ApiController]
[Route("[controller]")]
public class InventoryController : ControllerBase {
    private readonly ILogger<InventoryController> logger;
    private readonly InventoryService inventoryService;

    public InventoryController
    (
        ILogger<InventoryController> logger,
        InventoryService inventoryService
    )
    {
        this.logger = logger;
        this.inventoryService = inventoryService;
    }

    // Create
    [HttpPost]
    public async Task<ActionResult<Inventory>> Post
    (
        ulong playerId, ulong itemId
    )
    {
        Inventory inventory = new Inventory {
            PlayerId = playerId,
            ItemId   = itemId
        };

        Console.WriteLine("INVENTORY::::: " + inventory);

        await inventoryService.Create(inventory);
        if (inventory.Id != default)
            return CreatedAtRoute(
                "FindOneItem",
                new { id = inventory.Id },
                inventory
            );
        else
            return BadRequest();
    }

    // Update
    [HttpPut]
    public async Task<ActionResult<Inventory>> Put
    (
        ulong playerId, ulong itemId
    )
    {
        Inventory inventory = new Inventory {
            PlayerId = playerId,
            ItemId = itemId
        };

        var result = await inventoryService.Update(inventory);

        if (result > 0)
            return NoContent();
        else
            return NotFound();
    }

    // Read
    [HttpGet("{id}", Name = "FindOneInventory")]
    public async Task<ActionResult<Inventory>> Get(ulong id)
    {
        var result = await inventoryService.FindOne(id);
        if (result != default)
            return Ok(result);
        else
            return NotFound();
    }

    [HttpGet]
    public async Task<IEnumerable<Inventory>> Get() =>
        await inventoryService.FindAll();

    // Delete
    [HttpDelete]
    public async Task<ActionResult<Item>> Delete(ulong id)
    {
        var result = await inventoryService.Delete(id);
        if (result > 0)
            return NoContent();
        else
            return NotFound();
    }
}
