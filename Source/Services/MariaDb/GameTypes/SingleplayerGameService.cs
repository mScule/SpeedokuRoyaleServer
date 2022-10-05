using Microsoft.EntityFrameworkCore;
using SpeedokuRoyaleServer.Models.DbContexts;

namespace SpeedokuRoyaleServer.Models.Services.MariaDB;

public sealed class SingleplayerGameService : Service
{
    public SingleplayerGameService(MariaDbContext dbContext)
        : base(dbContext) { }

    // Create
    public async Task<ulong?> Create(SingleplayerGame game)
    {
        await dbContext.AddAsync(game);
        await dbContext.SaveChangesAsync();
        return game.Id;
    }

    // Read
    public async Task<IEnumerable<SingleplayerGame>> FindAll() =>
        await dbContext.SingleplayerGames.ToListAsync();

    public async Task<SingleplayerGame?> FindOne(ulong id)
    {
        SingleplayerGame? result = await dbContext.SingleplayerGames
            .FirstOrDefaultAsync(game => game.Id == id);

        return result;
    }

    // Update
    public async Task<int> Update(SingleplayerGame game)
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
            dbContext.SingleplayerGames.Remove(
                new SingleplayerGame { Id = id }
            );
            return await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return 0;
        }
    }
}
