using Microsoft.EntityFrameworkCore;
namespace SpeedokuRoyaleServer.Models;

public sealed class MariaDbTodoService
{
    private MariaDbContext dbContext;

    public MariaDbTodoService(MariaDbContext dbContext) =>
        this.dbContext = dbContext;

    // Create
    public async Task<long?> Create(TodoItem todoItem)
    {
        await dbContext.AddAsync(todoItem);
        await dbContext.SaveChangesAsync();
        return todoItem.Id;
    }

    // Read
    public async Task<IEnumerable<TodoItem>> FindAll() =>
        await dbContext.TodoItems.ToListAsync();

    public async Task<TodoItem?> FindOne(long id)
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
    public async Task<int> Delete(long id)
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
