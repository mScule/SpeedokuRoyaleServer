using Microsoft.AspNetCore.Mvc;
using SpeedokuRoyaleServer.Models;
using SpeedokuRoyaleServer.Models.Services.MariaDB;

namespace SpeedokuRoyaleServer.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoController : ControllerBase
{
    private readonly ILogger<TodoController> logger;
    private readonly TodoService todoService;

    public TodoController
    (
        ILogger<TodoController> logger,
        TodoService todoService
    )
    {
        this.logger = logger;
        this.todoService = todoService;
    }

    // Create
    [HttpPost]
    public async Task<ActionResult<TodoItem>> Post(string task, bool isComplete)
    {
        TodoItem todoItem = new TodoItem { Task = task, IsComplete = isComplete };

        await todoService.Create(todoItem);
        if (todoItem.Id != default)
            return CreatedAtRoute(
                "FindOneTodoItem",
                new { id = todoItem.Id },
                todoItem
            );
        else
            return BadRequest();
    }

    // Read
    [HttpGet("{id}", Name = "FindOneTodoItem")]
    public async Task<ActionResult<TodoItem>> Get(ulong id)
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
    public async Task<ActionResult<TodoItem>> Put([FromQuery] TodoItem todoItem)
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
    public async Task<ActionResult<TodoItem>> Delete(ulong id)
    {
        var result = await todoService.Delete(id);
        if (result > 0)
            return NoContent();
        else
            return NotFound();
    }
}
