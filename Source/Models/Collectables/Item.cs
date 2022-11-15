using System.ComponentModel.DataAnnotations;

namespace SpeedokuRoyaleServer.Models;

public class Item : Indexed
{
    [Required]
    public Rarity Rarity { get; set; }

    public ICollection<Inventory>? Inventories { get; set; }
}
