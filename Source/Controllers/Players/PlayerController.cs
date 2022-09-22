using Microsoft.AspNetCore.Mvc;
using SpeedokuRoyaleServer.Models;
using SpeedokuRoyaleServer.Models.Services.MariaDB;

namespace SpeedokuRoyaleServer.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayerController : ControllerBase
{
    private readonly ILogger<PlayerController> logger;
    private readonly PlayerService playerService;

    public PlayerController
    (
        ILogger<PlayerController> logger,
        PlayerService playerService
    )
    {
        this.logger = logger;
        this.playerService = playerService;
    }

    // Create
    [HttpPost]
    public async Task<ActionResult<Player>> Post([FromBody]Player player)
    {
        await playerService.Create(player);
        if (player.Id != default)
            return CreatedAtRoute(
                "FindOnePlayer",
                new { id = player.Id },
                player
            );
        else
            return BadRequest();
    }

    // Read
    [HttpGet("{id}", Name = "FindOnePlayer")]
    public async Task<ActionResult<Player>> Get(ulong id)
    {
        var result = await playerService.FindOne(id);
        if (result != default)
            return Ok(result);
        else
            return NotFound();
    }

    // Items

    [HttpGet]
    public async Task<IEnumerable<Player>> Get() =>
        await playerService.FindAll();

    // Update
    [HttpPut]
    public async Task<ActionResult<Player>> Put([FromBody] Player player)
    {
        if (player.Id == null)
            return BadRequest("Id has to be stated");

        var result = await playerService.Update(player);
        if (result > 0)
            return NoContent();
        else
            return NotFound();
    }

    // Delete
    [HttpDelete]
    public async Task<ActionResult<Player>> Delete(ulong id)
    {
        var result = await playerService.Delete(id);
        if (result > 0)
            return NoContent();
        else
            return NotFound();
    }
}
