using Microsoft.EntityFrameworkCore;

namespace SpeedokuRoyaleServer.Models.DbContexts;

public class MariaDbContext : DbContext
{
    public MariaDbContext(DbContextOptions options)
        : base(options) { }

    // Example Model
    public DbSet<TodoItem> TodoItems { get; set; } = null!;

    // Speedoku Royale Models
    public DbSet<Player>              Players             { get; set; } = null!;
    public DbSet<Item>                Items               { get; set; } = null!;
    public DbSet<Inventory>           Inventories         { get; set; } = null!;
    public DbSet<SingleplayerGame>    SingleplayerGames   { get; set; } = null!;
    public DbSet<MultiplayerGame>     MultiplayerGames    { get; set; } = null!;
    public DbSet<MultiplayerSession>  MultiplayerSessions { get; set; } = null!;

    // Associations
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Inventory

        // Linking Player with Inventory
        modelBuilder.Entity<Inventory>()
            .HasOne(i => i.Player)
            .WithMany(p => p.Inventories)
            .HasForeignKey(i => i.PlayerId);
        
        // Linking Item with Inventory
        modelBuilder.Entity<Inventory>()
            .HasOne(i => i.Item)
            .WithMany(i => i.Inventories)
            .HasForeignKey(i => i.ItemId);

        // Multiplayer session

        // Linking Player with Multiplayer sessions
        modelBuilder.Entity<MultiplayerSession>()
            .HasOne(ms => ms.Player)
            .WithMany(p => p.MultiplayerSessions)
            .HasForeignKey(ms => ms.PlayerId);

        // Linking Multiplayer games with Multiplayer sessions
        modelBuilder.Entity<MultiplayerSession>()
            .HasOne(ms => ms.MultiplayerGame)
            .WithMany(mg => mg.MultiplayerSessions)
            .HasForeignKey(ms => ms.MultiplayerGameId);
    }
}
