namespace SpeedokuRoyaleServer.Models;

public class Inventory : Indexed
{
    public ulong PlayerId { get; set; }
    public Player? Player { get; set; }

    public ulong ItemId { get; set; }
    public Item? Item { get; set; }
}
