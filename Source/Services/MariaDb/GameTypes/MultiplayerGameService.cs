using Microsoft.EntityFrameworkCore;
using SpeedokuRoyaleServer.Models.DbContexts;

namespace SpeedokuRoyaleServer.Models.Services.MariaDB;

public sealed class MultiplayerGameService : Service
{
    public MultiplayerGameService(MariaDbContext dbContext)
        : base(dbContext) { }

    // Create
    public async Task<ulong?> Create(MultiplayerGame game)
    {
        await dbContext.AddAsync(game);
        await dbContext.SaveChangesAsync();
        return game.Id;
    }

    // Read
    public async Task<IEnumerable<MultiplayerGame>> FindAll() =>
        await dbContext.MultiplayerGames.ToListAsync();

    public async Task<MultiplayerGame?> FindOne(ulong id)
    {
        MultiplayerGame? result = await dbContext.MultiplayerGames
            .FirstOrDefaultAsync(game => game.Id == id);

        return result;
    }

    // Update
    public async Task<int> Update(MultiplayerGame game)
    {
        try
        {
            dbContext.Update(game);
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
            dbContext.MultiplayerGames.Remove(
                new MultiplayerGame { Id = id }
            );
            return await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return 0;
        }
    }
}
