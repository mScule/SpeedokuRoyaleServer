using Microsoft.EntityFrameworkCore;
using SpeedokuRoyaleServer.Models.DbContexts;

namespace SpeedokuRoyaleServer.Models.Services.MariaDB;

public sealed class InventoryService : Service
{
    public InventoryService(MariaDbContext dbContext) : base(dbContext) { }

    // Create
    public async Task<ulong?> Create(Inventory inventory)
    {
        await dbContext.AddAsync(inventory);
        await dbContext.SaveChangesAsync();
        return inventory.Id;
    }

    // Read
    public async Task<IEnumerable<Inventory>> FindAll() =>
        await dbContext.Inventories.ToListAsync();

    public async Task<Inventory?> FindOne(ulong id)
    {
        Inventory? result = await dbContext.Inventories
            .FirstOrDefaultAsync(token => token.Id == id);

        return result;
    }

    // Update
    public async Task<int> Update(Inventory inventory)
    {
        try
        {
            dbContext.Update(inventory);
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
            dbContext.Inventories.Remove(new Inventory { Id = id });
            return await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return 0;
        }
    }
}