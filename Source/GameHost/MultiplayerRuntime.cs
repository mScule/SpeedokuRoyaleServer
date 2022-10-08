using SpeedokuRoyaleServer.Utility;

namespace SpeedokuRoyaleServer.GameHost;

public class MultiplayerRuntime
{
    // Config
    public string RoomName   { set; get; } = "";
    public int    GameLength { set; get; } = 2;
    public byte   RoomSize   { set; get; } = 0;

    // State
    public RuntimeState State { set; get; } = RuntimeState.WaitingForPlayers;

    // Runtime - private
    private DateTime? startTime = null, endTime = null, now = null;
    private List<ulong> players = new List<ulong>();
    private Dictionary<ulong, ulong> scores = new Dictionary<ulong, ulong>();

    // Runtime - public
    public DateTime? StartTime { get => startTime; }
    public KeyValuePair<ulong, ulong>[] Scores {
        get => scores.ToArray<KeyValuePair<ulong,ulong>>();
    }
    public ulong? Winner { get; set; } = null;

    // Methods
    public int PlayerAmt() => players.Count;

    public bool HasPlayer(ulong playerId) {
        foreach (ulong id in players)
            if (playerId == id)
                return true;
        return false;
    }

    public void ClearRoom() {
        Console.WriteLine($"Clearing room {RoomName}");
        this.players.Clear();
        this.scores.Clear();

        this.RoomName = IdGenerator.NewId();
        this.State    = RuntimeState.WaitingForPlayers;
    }

    public void AddPlayer(ulong playerId)
    {
        this.players.Add(playerId);

        Console.WriteLine($"Player {playerId} joined room {RoomName}!");

        if (this.players.Count >= RoomSize)
        {
            this.startTime = new DateTime(DateTime.Now.Ticks);

            Console.WriteLine($"Game starts in room {RoomName}");
            this.State = RuntimeState.InGame;
            this.endTime = new DateTime(DateTime.Now.Ticks).AddMinutes(GameLength);

            foreach(ulong id in players)
            {
                this.scores.Add(id, 0);
            }
        }
    }

    public void FinishGame()
    {
        ulong? winner = null;

        ulong highestScore = 0;

        foreach(KeyValuePair<ulong, ulong> player in scores)
        {
            if(player.Value > highestScore)
            {
                highestScore = player.Value;
                winner = player.Key;
            }
        }

        Console.WriteLine
        (
            $"Gameroom {RoomName} game ended. Winner playerid: {winner}"
        );

        this.State  = RuntimeState.Finished;
        this.Winner = winner;
    }

    public void UpdateTimer()
    {
        now = new DateTime(DateTime.Now.Ticks);
        TimeSpan? timeLeft = endTime - now;
        Console.WriteLine($"Time remaining {timeLeft} for room {RoomName}");

        if (now >= this.endTime)
            FinishGame();
    }

    public void AddScore(ulong playerId, ulong amt)
    {
        UpdateTimer();

        if (now < this.endTime)
        {
            this.scores[playerId] += amt;
        }
    }
}
