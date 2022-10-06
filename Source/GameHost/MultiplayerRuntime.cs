using System.Collections.Generic;
using SpeedokuRoyaleServer.Models.Services.MariaDB;

namespace SpeedokuRoyaleServer.GameHost;

public class MultiplayerRuntime
{
    private static readonly int GameLength = 2;

    public string RoomName { set; get; } = "";
    public byte RoomSize { set; get; } = 0;

    public RuntimeState State { set; get; } = RuntimeState.WaitingForPlayers;
    private DateTime? endTime = null;

    private List<ulong> players = new List<ulong>();
    private Dictionary<ulong, ulong> scores = new Dictionary<ulong, ulong>();

    public int PlayerAmt()
    {
        return players.Count;
    }

    public bool HasPlayer(ulong playerId) {
        foreach (ulong id in players)
        {
            if (playerId == id)
                return true;
        }
        return false;
    }

    public void AddPlayer(ulong playerId)
    {
        this.players.Add(playerId);

        Console.WriteLine($"Player {playerId} joined room {RoomName}!");

        if (this.players.Count >= RoomSize)
        {
            Console.WriteLine($"Game starts in room {RoomName}");
            this.State = RuntimeState.InGame;
            this.endTime = new DateTime(DateTime.Now.Ticks).AddMinutes(GameLength);

            foreach(ulong id in players)
            {
                this.scores.Add(id, 0);
            }
        }
    }

    public void AddScore(ulong playerId)
    {
        DateTime now = new DateTime(DateTime.Now.Ticks);

        if (DateTime.Now < this.endTime)
        {
            TimeSpan? timeLeft = endTime - now;
            Console.WriteLine(
                $"Player {playerId} got a score! Time remaining {timeLeft}"
            );
            this.scores[playerId]++;
        }
        else
        {
            KeyValuePair<ulong, ulong> winner = new KeyValuePair<ulong, ulong>(0,0);
            ulong highestScore = 0;

            foreach(KeyValuePair<ulong, ulong> player in scores)
            {
                if(player.Value > highestScore)
                {
                    highestScore = player.Value;
                    winner = player;
                }
            }

            Console.WriteLine($"Gameroom {RoomName} game ended. Winner playerid: {winner.Key}");
            this.State = RuntimeState.Finished;
        }
    }
}