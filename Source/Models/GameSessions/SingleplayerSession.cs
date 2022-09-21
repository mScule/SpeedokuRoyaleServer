namespace SpeedokuRoyaleServer.Models;

public class SingleplayerSession : Indexed
{
    public ulong SingleplayerGameId { get; set; }
    public SingleplayerGame? SingleplayerGame { get; set; }

    public ulong PlayerId { get; set; }
    public Player? Player { get; set; }
}
