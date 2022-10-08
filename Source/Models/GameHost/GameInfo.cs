namespace SpeedokuRoyaleServer.Models;

public class GameInfo
{
    public RuntimeState State { get; set; }
    public Tuple<ulong, ulong>[]? Players { get; set; } = null;
}
