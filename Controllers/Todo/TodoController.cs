using Microsoft.AspNetCore.Mvc;
using SpeedokuRoyaleServer.Models;

namespace SpeedokuRoyaleServer.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoController : ControllerBase
{
    private readonly ILogger<TodoController> logger;
    private readonly MariaDbTodoService todoService;

    public TodoController
    (
        ILogger<TodoController> logger,
        MariaDbTodoService todoService
    )
    {
        this.logger = logger;
        this.todoService = todoService;
    }

    // Create
    [HttpPost]
    public async Task<ActionResult<TodoItem>> Post(TodoItem todoItem)
    {
        if (todoItem.Id != null)
            return BadRequest("Id is automatically incremented");

        await todoService.Create(todoItem);
        if (todoItem.Id != default)
            return CreatedAtRoute(
                "FindOne",
                new { id = todoItem.Id },
                todoItem
            );
        else
            return BadRequest();
    }

    // Read
    [HttpGet("{id}", Name = "FindOne")]
    public async Task<ActionResult<TodoItem>> Get(long id)
    {
        var result = await todoService.FindOne(id);
        if (result != default)
            return Ok(result);
        else
            return NotFound();
    }

    [HttpGet]
    public async Task<IEnumerable<TodoItem>> Get() =>
        await todoService.FindAll();

    // Update
    [HttpPut]
    public async Task<ActionResult<TodoItem>> Put(TodoItem todoItem)
    {
        if (todoItem.Id == null)
            return BadRequest("Id has to be stated");

        var result = await todoService.Update(todoItem);
        if (result > 0)
            return NoContent();
        else
            return NotFound();
    }

    // Delete
    [HttpDelete]
    public async Task<ActionResult<TodoItem>> Delete(long id)
    {
        var result = await todoService.Delete(id);
        if (result > 0)
            return NoContent();
        else
            return NotFound();
    }
}
