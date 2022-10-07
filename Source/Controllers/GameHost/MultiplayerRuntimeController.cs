using Microsoft.AspNetCore.Mvc;

using SpeedokuRoyaleServer.GameHost;
using SpeedokuRoyaleServer.Models.Services.MariaDB;
using SpeedokuRoyaleServer.Models;

namespace SpeedokuRoyaleServer.Controllers;

[ApiController]
[Route("[controller]")]
public class MultiplayerRuntimeController : ControllerBase
{
    private readonly ILogger<MultiplayerRuntimeController> logger;

    private readonly MultiplayerSessionService multiplayerSessionService;
    private readonly MultiplayerGameService multiplayerGameService;
    private readonly PlayerService playerService;

    public MultiplayerRuntimeController
    (
        ILogger<MultiplayerRuntimeController> logger,

        MultiplayerSessionService multiplayerSessionService,
        MultiplayerGameService multiplayerGameService,
        PlayerService playerService
    )
    {
        this.logger = logger;

        this.multiplayerSessionService = multiplayerSessionService;
        this.multiplayerGameService = multiplayerGameService;
        this.playerService = playerService;
    }

    // Gamerooms are being hardcoded for now...
    private static MultiplayerRuntime[] gameRooms = new MultiplayerRuntime[] {
        new MultiplayerRuntime {
            RoomName   = "Room1",
            RoomSize   = 2,
            GameLength = 3
        },
        new MultiplayerRuntime {
            RoomName   = "Room2",
            RoomSize   = 2,
            GameLength = 3
        },
         new MultiplayerRuntime {
            RoomName   = "Room3",
            RoomSize   = 2,
            GameLength = 3
        },
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
    public async Task<ActionResult<bool>> AddScore
    (
        ulong  playerId,
        string roomName
    )
    {
        bool scoreAddedSuccessfully = false;

        await Task.Run(async () =>
        {
            foreach (MultiplayerRuntime room in gameRooms)
            {
                if
                (
                    room.RoomName == roomName &&
                    room.HasPlayer(playerId) &&
                    room.State == RuntimeState.InGame
                )
                {
                    room.AddScore(playerId);
                    scoreAddedSuccessfully = true;
                    break;
                }
                else if
                (
                    room.RoomName == roomName &&
                    room.State == RuntimeState.Finished &&
                    room.StartTime != null
                )
                {
                    ulong? gameId = await multiplayerGameService.Create
                    (
                        new MultiplayerGame { Date = room.StartTime }
                    );

                    foreach (KeyValuePair<ulong, ulong> player in room.Scores)
                    {
                        await multiplayerSessionService.Create
                        (
                            new MultiplayerSession
                            {
                                MultiplayerGameId = (ulong)gameId,
                                PlayerId = player.Key,
                                Score = player.Value
                            }
                        );
                    }
                    room.ClearRoom();
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
