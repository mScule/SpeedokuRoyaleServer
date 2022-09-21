using Microsoft.EntityFrameworkCore;
using SpeedokuRoyaleServer.Models.DbContexts;

namespace SpeedokuRoyaleServer.Models.Services.MariaDB;

public sealed class ItemService
{
    private MariaDbContext dbContext;

    public ItemService(MariaDbContext dbContext) =>
        this.dbContext = dbContext;

    // Create
    public async Task<ulong?> Create(Item item)
    {
        await dbContext.AddAsync(item);
        await dbContext.SaveChangesAsync();
        return item.Id;
    }

    // Read
    public async Task<IEnumerable<Item>> FindAll() =>
        await dbContext.Items.ToListAsync();

    public async Task<Item?> FindOne(ulong id)
    {
        Item? result = await dbContext.Items.FirstOrDefaultAsync(
            token => token.Id == id
        );
        return result;
    }

    // Update
    public async Task<int> Update(Item item)
    {
        try
        {
            dbContext.Update(item);
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
            dbContext.Items.Remove(new Item { Id = id });
            return await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return 0;
        }
    }
}
