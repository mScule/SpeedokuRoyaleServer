namespace SpeedokuRoyaleServer.Models;

public class MultiplayerSession : Indexed
{
    public ulong Score { get; set; }

    public ulong MultiplayerGameId { get; set; }
    public MultiplayerGame? MultiplayerGame { get; set; }

    public ulong PlayerId { get; set; }
    public Player? Player { get; set; }
}
