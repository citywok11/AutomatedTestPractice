using Microsoft.AspNetCore.Mvc;
using TodoApi.Core.Interfaces;
using TodoApi.API.DTOs;

namespace TodoApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly ITodoService _todoService;

    public TodosController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpPost]
    public async Task<ActionResult<TodoResponse>> CreateTodo([FromBody] CreateTodoRequest request, [FromQuery] int userId)
    {
        try
        {
            var todo = await _todoService.CreateTodoAsync(userId, request.Title, request.Description);
            var response = new TodoResponse(todo.Id, todo.Title, todo.Description, todo.IsCompleted, todo.CreatedAt, todo.UserId);
            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoResponse>> GetTodo(int id)
    {
        // This method is needed for CreatedAtAction but we'll implement it for completeness
        return Ok(); // Placeholder - in full implementation this would return the todo
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<TodoResponse>> UpdateTodo(int id, [FromBody] UpdateTodoRequest request)
    {
        var todo = await _todoService.MarkTodoCompleteAsync(id);
        if (todo == null)
        {
            return NotFound("Todo not found");
        }

        var response = new TodoResponse(todo.Id, todo.Title, todo.Description, todo.IsCompleted, todo.CreatedAt, todo.UserId);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTodo(int id)
    {
        var deleted = await _todoService.DeleteTodoAsync(id);
        if (!deleted)
        {
            return NotFound("Todo not found");
        }

        return NoContent();
    }
}