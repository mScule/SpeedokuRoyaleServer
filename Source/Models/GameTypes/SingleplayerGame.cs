namespace SpeedokuRoyaleServer.Models;

public class SingleplayerGame : Game {
    public ulong Score    { get; set; }
    public ulong PlayerId { get; set; }
    public Player? Player { get; set; }
}
