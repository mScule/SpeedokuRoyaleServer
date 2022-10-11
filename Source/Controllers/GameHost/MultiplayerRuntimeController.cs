using Microsoft.AspNetCore.Mvc;

using SpeedokuRoyaleServer.GameHost;
using SpeedokuRoyaleServer.Models.Services.MariaDB;
using SpeedokuRoyaleServer.Models;
using SpeedokuRoyaleServer.Utility;

namespace SpeedokuRoyaleServer.Controllers;

[ApiController]
[Route("[controller]")]
public class MultiplayerRuntimeController : ControllerBase
{
    private static readonly int GeneralGameLength = 5;
    private static readonly byte GeneralRoomSize = 5;

    private readonly ILogger<MultiplayerRuntimeController> logger;

    private readonly MultiplayerSessionService multiplayerSessionService;
    private readonly MultiplayerGameService multiplayerGameService;
    private readonly PlayerService playerService;
    private readonly ItemService itemService;

    public MultiplayerRuntimeController
    (
        ILogger<MultiplayerRuntimeController> logger,

        MultiplayerSessionService multiplayerSessionService,
        MultiplayerGameService multiplayerGameService,
        PlayerService playerService,
        ItemService itemService
    )
    {
        this.logger = logger;

        this.multiplayerSessionService = multiplayerSessionService;
        this.multiplayerGameService    = multiplayerGameService;
        this.playerService             = playerService;
        this.itemService               = itemService;

        foreach(MultiplayerRuntime room in gameRooms) {
            room.Logger = this.logger;
        }
    }

    // Gamerooms are being hardcoded for now...
    private static MultiplayerRuntime[] gameRooms = new MultiplayerRuntime[] {
        new MultiplayerRuntime {
            RoomName   = IdGenerator.NewId(),
            RoomSize   = GeneralRoomSize,
            GameLength = GeneralGameLength
        },
        new MultiplayerRuntime {
            RoomName   = IdGenerator.NewId(),
            RoomSize   = GeneralRoomSize,
            GameLength = GeneralGameLength
        },
        new MultiplayerRuntime {
            RoomName   = IdGenerator.NewId(),
            RoomSize   = GeneralRoomSize,
            GameLength = GeneralGameLength
        },
    };

    private static List<string> closedGames = new List<string>();

    private void EndGame(MultiplayerRuntime gameRoom)
    {
        ulong? winner = gameRoom.Winner;

        // Giving price for the winner
        if (winner != null)
        {
            // TODO: Add random price for the player
            logger.LogInformation("Add Item for winner for the winner...");
        }

        closedGames.Add(gameRoom.RoomName + "");
        gameRoom.ClearRoom();

        // Showing closed games
        string closedGamesInfo = "Closed games:\n";
        foreach(string game in closedGames)
            closedGamesInfo += game + "\n";

        this.logger.LogInformation(closedGamesInfo);
    }

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

    [HttpPost("{roomName}/Kill")]
    public async Task<ActionResult<bool>> Kill(ulong playerId, string roomName)
    {
        bool killingSuccessfull = false;

        await Task.Run(() => {
            foreach (MultiplayerRuntime room in gameRooms)
            {
                if
                (
                    room.RoomName == roomName &&
                    room.State == RuntimeState.InGame
                )
                {
                    killingSuccessfull = room.KillPlayer(playerId);
                }
            }
        });

        return Ok(killingSuccessfull);
    }


    [HttpGet("{roomName}/Status")]
    public async Task<ActionResult<RuntimeState>> Status(string roomName)
    {
        RuntimeState state = RuntimeState.WaitingForPlayers;
        bool found = false;

        await Task.Run(async () =>
        {
            // Trying to find the room from active ones...
            foreach (MultiplayerRuntime room in gameRooms)
            {
                if (room.State == RuntimeState.InGame)
                    room.UpdateTimer();

                if (room.State == RuntimeState.Finished)
                {
                    // Adding game
                    ulong? gameId = await multiplayerGameService.Create
                    (
                        new MultiplayerGame { Date = room.StartTime }
                    );

                    // Adding scores to players
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

                    EndGame(room);
                    break;
                }

                if (room.RoomName == roomName)
                {
                    state = room.State;
                    found = true;
                    break;
                }
            }

            // Trying to find the room from the closed ones...
            if (!found)
            {
                foreach (string name in closedGames) {
                    if (roomName == name) {
                        state = RuntimeState.Closed;
                        found = true;
                        break;
                    }
                }
            }
        });

        if (found)
            return Ok(state);
        else
            return NotFound();
    }

    [HttpGet("{roomName}/InGameStatus")]
    public async Task<ActionResult<GameInfo>> InGameStatus(string roomName)
    {
        bool found = false;

        GameInfo? gameInfo = null;

        foreach(MultiplayerRuntime room in gameRooms)
        {
            if (room.State == RuntimeState.InGame)
                    room.UpdateTimer();

            if (room.State == RuntimeState.Finished)
            {
                // Adding game
                ulong? gameId = await multiplayerGameService.Create
                (
                    new MultiplayerGame { Date = room.StartTime }
                );

                // Adding scores to players
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

                EndGame(room);
                break;
            }

            if (room.RoomName == roomName && room.State == RuntimeState.InGame)
            {
                gameInfo = new GameInfo
                {
                    State = room.State,
                    Players = room.ScoreInfo()
                };

                found = true;
                break;
            }
        }

        // Trying to find the room from the closed ones...
        if (!found)
        {
            foreach (string name in closedGames) {
                if (roomName == name) {
                    gameInfo = new GameInfo
                    {
                        Players = null,
                        State = RuntimeState.Closed
                    };
                    found = true;
                    break;
                }
            }
        }

        if (found && gameInfo != null)
            return Ok(gameInfo);
        else
            return NotFound();
    }

    [HttpPost("{roomName}/AddScore")]
    public async Task<ActionResult<bool>> AddScore
    (
        ulong  playerId,
        ulong  scores,
        string roomName
    )
    {
        bool scoreAddedSuccessfully = false;

        await Task.Run(async() =>
        {
            foreach (MultiplayerRuntime room in gameRooms)
            {
                if
                (
                    room.RoomName == roomName &&
                    room.HasPlayer(playerId)  &&
                    room.State == RuntimeState.InGame
                )
                {
                    room.AddScore(playerId, scores);
                    scoreAddedSuccessfully = true;
                    break;
                }

                if
                (
                    room.RoomName == roomName &&
                    room.State == RuntimeState.Finished &&
                    room.StartTime != null
                )
                {
                    // Adding game
                    ulong? gameId = await multiplayerGameService.Create
                    (
                        new MultiplayerGame { Date = room.StartTime }
                    );

                    // Adding scores to players
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

                    EndGame(room);
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
                        Name    = room.RoomName,
                        Players = room.PlayerAmt(),
                        Size    = room.RoomSize,
                        State   = room.State
                    });
                }
            }
        });

        return Ok(avaliableRooms.ToArray());
    }
}
