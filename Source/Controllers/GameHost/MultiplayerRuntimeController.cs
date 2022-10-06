using Microsoft.AspNetCore.Mvc;
using SpeedokuRoyaleServer.GameHost;
using SpeedokuRoyaleServer.Models;

namespace SpeedokuRoyaleServer.Controllers;

[ApiController]
[Route("[controller]")]
public class MultiplayerRuntimeController : ControllerBase
{
    private readonly ILogger<MultiplayerRuntimeController> logger;

    public MultiplayerRuntimeController
    (
        ILogger<MultiplayerRuntimeController> logger
    )
    {
        this.logger = logger;
    }

    // Gamerooms are being hardcoded for now...
    private static MultiplayerRuntime[] gameRooms = new MultiplayerRuntime[] {
        new MultiplayerRuntime { RoomName = "Room1", RoomSize = 2 },
        new MultiplayerRuntime { RoomName = "Room2", RoomSize = 2 },
        new MultiplayerRuntime { RoomName = "Room3", RoomSize = 2 },
    };

    [HttpPost("{roomName}/Join")]
    public async Task<ActionResult<bool>> Join(ulong playerId, string roomName)
    {
        bool joinSuccess = false;

        await Task.Run(() =>
        {
            foreach (MultiplayerRuntime room in gameRooms)
            {
                if
                (
                    room.RoomName == roomName &&
                    room.State == RuntimeState.WaitingForPlayers
                )
                {
                    room.AddPlayer(playerId);
                    joinSuccess = true;
                    break;
                }
            }
        });

        return Ok(joinSuccess);
    }

    [HttpGet("{roomName}/Status")]
    public async Task<ActionResult<RuntimeState>> Status(string roomName)
    {
        RuntimeState state = RuntimeState.WaitingForPlayers;
        bool found = false;

        await Task.Run(() =>
        {
            foreach (MultiplayerRuntime room in gameRooms)
            {
                if (room.RoomName == roomName)
                {
                    state = room.State;
                    found = true;
                    break;
                }
            }
        });

        if (found)
        {
            return Ok(state);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPost("{roomName}/AddScore")]
    public async Task<ActionResult<bool>> AddScore(ulong playerId, string roomName)
    {
        bool scoreAddedSuccessfully = false;

        await Task.Run(() => {
            foreach (MultiplayerRuntime room in gameRooms)
            {
                if (room.RoomName == roomName && room.HasPlayer(playerId)) {
                    room.AddScore(playerId);
                    scoreAddedSuccessfully = true;
                    break;
                }
            }
        });

        return Ok(scoreAddedSuccessfully);
    }

    [HttpGet("AvaliableRooms")]
    public async Task<ActionResult<RoomInfo[]>> Get()
    {
        List<RoomInfo> avaliableRooms = new List<RoomInfo>();

        await Task.Run(() =>
        {
            foreach (MultiplayerRuntime room in gameRooms)
            {
                if (room.State == RuntimeState.WaitingForPlayers)
                {
                    avaliableRooms.Add(new RoomInfo
                    {
                        Name = room.RoomName,
                        Players = room.PlayerAmt(),
                        Size = room.RoomSize
                    });
                }
            }
        });

        return Ok(avaliableRooms.ToArray());
    }
}
