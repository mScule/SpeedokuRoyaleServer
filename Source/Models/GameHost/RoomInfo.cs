namespace SpeedokuRoyaleServer.Models;

public class RoomInfo
{
    public string       Name    { get; set; } = "";
    public int          Players { get; set; }
    public int          Size    { get; set; }
    public RuntimeState State   { get; set; }
}
