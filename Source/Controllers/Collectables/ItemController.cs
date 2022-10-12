using Microsoft.AspNetCore.Mvc;
using SpeedokuRoyaleServer.Models;
using SpeedokuRoyaleServer.Models.Services.MariaDB;

namespace SpeedokuRoyaleServer.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemController : ControllerBase {
    private readonly ILogger<ItemController> logger;
    private readonly ItemService itemService;

    public ItemController
    (
        ILogger<ItemController> logger,
        ItemService itemService
    )
    {
        this.logger = logger;
        this.itemService = itemService;
    }

    // Create
    [HttpPost]
    public async Task<ActionResult<Item>> Post
    (
        Rarity rarity
    )
    {
        Item item = new Item { Rarity = rarity };

        await itemService.Create(item);
        if (item.Id != default)
            return CreatedAtRoute(
                "FindOneItem",
                new { id = item.Id},
                item
            );
        else
            return BadRequest();
    }

    // Read
    [HttpGet("{id}", Name = "FindOneItem")]
    public async Task<ActionResult<Item>> Get(ulong id)
    {
        var result = await itemService.FindOne(id);
        if (result != default)
            return Ok(result);
        else
            return NotFound();
    }

    [HttpGet]
    public async Task<IEnumerable<Item>> Get() =>
        await itemService.FindAll();

    // Delete
    [HttpDelete]
    public async Task<ActionResult<Item>> Delete(ulong id)
    {
        var result = await itemService.Delete(id);
        if (result > 0)
            return NoContent();
        else
            return NotFound();
    }
}
