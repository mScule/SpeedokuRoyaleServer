using System.ComponentModel.DataAnnotations;

namespace SpeedokuRoyaleServer.Models;

public class Player : Indexed
{
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Password { get; set; }

    public virtual ICollection<Inventory>? Inventories { get; set; }
    public virtual ICollection<SingleplayerSession>? SingleplayerSessions { get; set; }
    public virtual ICollection<MultiplayerSession>?  MultiplayerSessions  { get; set; }
}
