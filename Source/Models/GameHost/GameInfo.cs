namespace SpeedokuRoyaleServer.Models;

public class GameInfo
{
    public RuntimeState State { get; set; }
    public ScoreData[]? Players { get; set; } = null;
}
