using Microsoft.EntityFrameworkCore;
using SpeedokuRoyaleServer.Models.DbContexts;

namespace SpeedokuRoyaleServer.Models.Services.MariaDB;

public sealed class TodoService
{
    private MariaDbContext dbContext;

    public TodoService(MariaDbContext dbContext) =>
        this.dbContext = dbContext;

    // Create
    public async Task<ulong?> Create(TodoItem todoItem)
    {
        await dbContext.AddAsync(todoItem);
        await dbContext.SaveChangesAsync();
        return todoItem.Id;
    }

    // Read
    public async Task<IEnumerable<TodoItem>> FindAll() =>
        await dbContext.TodoItems.ToListAsync();

    public async Task<TodoItem?> FindOne(ulong id)
    {
        TodoItem? result = await dbContext.TodoItems.FirstOrDefaultAsync(
            token => token.Id == id
        );
        return result;
    }

    // Update
    public async Task<int> Update(TodoItem todoItem)
    {
        try
        {
            dbContext.Update(todoItem);
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
            dbContext.TodoItems.Remove(new TodoItem { Id = id });
            return await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return 0;
        }
    }
}
