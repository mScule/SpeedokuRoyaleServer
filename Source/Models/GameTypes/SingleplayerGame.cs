namespace SpeedokuRoyaleServer.Models;

public class SingleplayerGame : Game {
    public virtual ICollection<SingleplayerSession>? SingleplayerSessions { get; set; }
}
