namespace SpeedokuRoyaleServer.Models;

public class GameInfo
{
    public Player[]? Players { get; set; } = null;
    public KeyValuePair<ulong, ulong>[]? Scores { get; set; } = null;
    
}
