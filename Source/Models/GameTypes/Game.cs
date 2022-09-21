namespace SpeedokuRoyaleServer.Models;

public class Game : Indexed
{
    DateTime StartTime { get; set; }
    uint Duration { get; set; }
}
