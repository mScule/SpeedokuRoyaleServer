using Microsoft.EntityFrameworkCore;

namespace SpeedokuRoyaleServer.Models;

public class MariaDbContext : DbContext
{
    public MariaDbContext(DbContextOptions options)
        : base(options)
    {
    }

    // Models
    public DbSet<TodoItem> TodoItems { get; set; } = null!; // Example
}
