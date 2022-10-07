using Microsoft.AspNetCore.Mvc;
using SpeedokuRoyaleServer.Models;
using SpeedokuRoyaleServer.Models.Services.MariaDB;

namespace SpeedokuRoyaleServer.Controllers;

[ApiController]
[Route("[controller]")]
public class MultiplayerGameController : ControllerBase
{
    private readonly ILogger<MultiplayerGameController> logger;
    private readonly MultiplayerGameService multiplayerGameService;

    public MultiplayerGameController
    (
        ILogger<MultiplayerGameController> logger,
        MultiplayerGameService multiplayerGameService
    )
    {
        this.logger = logger;
        this.multiplayerGameService = multiplayerGameService;
    }

    // Read
    [HttpGet("{id}", Name = "FindOneMultiplayerGame")]
    public async Task<ActionResult<MultiplayerGame>> Get(ulong id)
    {
        var result = await multiplayerGameService.FindOne(id);
        if (result != default)
            return Ok(result);
        else
            return NotFound();
    }

    [HttpGet]
    public async Task<IEnumerable<MultiplayerGame>> Get() =>
        await multiplayerGameService.FindAll();
}
