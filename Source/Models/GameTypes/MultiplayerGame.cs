namespace SpeedokuRoyaleServer.Models;

public class MultiplayerGame : Game {
    public virtual ICollection<MultiplayerSession>? MultiplayerSessions { get; set; }
}
