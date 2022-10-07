using Microsoft.AspNetCore.Mvc;
using SpeedokuRoyaleServer.Models;
using SpeedokuRoyaleServer.Models.Services.MariaDB;

namespace SpeedokuRoyaleServer.Controllers;

[ApiController]
[Route("[controller]")]
public class SingleplayerGameController : ControllerBase
{
    private readonly ILogger<SingleplayerGameController> logger;
    private readonly SingleplayerGameService singleplayerGameService;

    public SingleplayerGameController
    (
        ILogger<SingleplayerGameController> logger,
        SingleplayerGameService service
    )
    {
        this.logger = logger;
        this.singleplayerGameService = service;
    }

    // Create
    [HttpPost]
    public async Task<ActionResult<SingleplayerGame>> Post
    (
        ulong score,
        ulong playerId
    )
    {
        SingleplayerGame game = new SingleplayerGame
        {
            Score = score,
            PlayerId = playerId
        };

        await singleplayerGameService.Create(game);
        if (game.Id != default)
            return CreatedAtRoute(
                "FindOneSingleplayerGame",
                new { id = game.Id },
                game
            );
        else
            return BadRequest();
    }

    // Read
    [HttpGet("{id}", Name = "FindOneSingleplayerGame")]
    public async Task<ActionResult<SingleplayerGame>> Get(ulong id)
    {
        var result = await singleplayerGameService.FindOne(id);
        if (result != default)
            return Ok(result);
        else
            return NotFound();
    }

    [HttpGet]
    public async Task<IEnumerable<SingleplayerGame>> Get() =>
        await singleplayerGameService.FindAll();
}
