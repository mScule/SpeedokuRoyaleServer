using Microsoft.EntityFrameworkCore;
using SpeedokuRoyaleServer.Models.DbContexts;

namespace SpeedokuRoyaleServer.Models.Services.MariaDB;

public sealed class MultiplayerSessionService : Service
{
    public MultiplayerSessionService(MariaDbContext dbContext)
        : base(dbContext) { }

    // Create
    public async Task<ulong?> Create(MultiplayerSession session)
    {
        await dbContext.AddAsync(session);
        await dbContext.SaveChangesAsync();
        return session.Id;
    }

    // Read
    public async Task<IEnumerable<MultiplayerSession>> FindAll() =>
        await dbContext.MultiplayerSessions.ToListAsync();

    public async Task<MultiplayerSession?> FindOne(ulong id)
    {
        MultiplayerSession? result = await dbContext.MultiplayerSessions
            .FirstOrDefaultAsync(session => session.Id == id);

        return result;
    }

    // Update
    public async Task<int> Update(MultiplayerSession session)
    {
        try
        {
            dbContext.Update(session);
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
            dbContext.MultiplayerSessions.Remove(
                new MultiplayerSession { Id = id }
            );
            return await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return 0;
        }
    }
}
