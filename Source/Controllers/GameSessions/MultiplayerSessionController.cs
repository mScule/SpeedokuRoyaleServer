using Microsoft.AspNetCore.Mvc;
using SpeedokuRoyaleServer.Models;
using SpeedokuRoyaleServer.Models.Services.MariaDB;

namespace SpeedokuRoyaleServer.Controllers;

[ApiController]
[Route("[controller]")]
public class MultiplayerSessionController : ControllerBase {
    private readonly ILogger<MultiplayerSessionController> logger;
    private readonly MultiplayerSessionService multiplayerSessionService;

    public MultiplayerSessionController
    (
        ILogger<MultiplayerSessionController> logger,
        MultiplayerSessionService multiplayerSessionService
    )
    {
        this.logger = logger;
        this.multiplayerSessionService = multiplayerSessionService;
    }

    // Read
    [HttpGet("{id}", Name = "FindOneMultiplayerSession")]
    public async Task<ActionResult<MultiplayerSession>> Get(ulong id)
    {
        var result = await multiplayerSessionService.FindOne(id);
        if (result != default)
            return Ok(result);
        else
            return NotFound();
    }

    [HttpGet]
    public async Task<IEnumerable<MultiplayerSession>> Get() =>
        await multiplayerSessionService.FindAll();
}
