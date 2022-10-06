using Microsoft.EntityFrameworkCore;
using SpeedokuRoyaleServer.Models.DbContexts;

namespace SpeedokuRoyaleServer.Models.Services.MariaDB;

public sealed class PlayerService
{
    private MariaDbContext dbContext;

    public PlayerService(MariaDbContext dbContext) =>
        this.dbContext = dbContext;

    public async Task<ulong?> Login(Player player)
    {
        Player? result = await dbContext.Players
            .FirstOrDefaultAsync(
                p => p.Email == player.Email &&
                p.Password == player.Password
            );
        return result != null ? result.Id : default;
    }

    // Create
    public async Task<ulong?> Create(Player player)
    {
        await dbContext.AddAsync(player);
        await dbContext.SaveChangesAsync();
        return player.Id;
    }

    // Read
    public async Task<IEnumerable<Player>> FindAll() =>
        await dbContext.Players
            .Include(p => p.Inventories)
            .ToListAsync();

    public async Task<Player?> FindOne(ulong id)
    {
        Player? result = await dbContext.Players
            .FirstOrDefaultAsync(token => token.Id == id);

        return result;
    }

    // Update
    public async Task<int> Update(Player player)
    {
        try
        {
            dbContext.Update(player);
            return await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return 0;
        }
    }

    // Delete
    public async Task<int> Delete(ulong id)
    {
        try
        {
            dbContext.Players.Remove(new Player { Id = id });
            return await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return 0;
        }
    }
}
