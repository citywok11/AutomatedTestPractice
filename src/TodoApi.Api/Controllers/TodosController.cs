using Microsoft.AspNetCore.Mvc;
using TodoApi.Core.DTOs;
using TodoApi.Core.Entities;
using TodoApi.Core.Interfaces;

namespace TodoApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly ITodoRepository _todoRepository;
    private readonly IUserRepository _userRepository;

    public TodosController(ITodoRepository todoRepository, IUserRepository userRepository)
    {
        _todoRepository = todoRepository;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Add a new todo item
    /// </summary>
    /// <param name="request">Todo creation request</param>
    /// <returns>Created todo</returns>
    [HttpPost]
    public async Task<ActionResult<TodoResponse>> CreateTodo(CreateTodoRequest request)
    {
        // Validate that user exists
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            return BadRequest(new { message = "User does not exist" });
        }

        var todo = new TodoItem
        {
            Title = request.Title,
            Description = request.Description,
            UserId = request.UserId,
            IsCompleted = false
        };

        var createdTodo = await _todoRepository.CreateAsync(todo);

        var response = new TodoResponse
        {
            Id = createdTodo.Id,
            Title = createdTodo.Title,
            Description = createdTodo.Description,
            IsCompleted = createdTodo.IsCompleted,
            CreatedAt = createdTodo.CreatedAt,
            CompletedAt = createdTodo.CompletedAt,
            UserId = createdTodo.UserId
        };

        return CreatedAtAction(nameof(GetTodo), new { id = createdTodo.Id }, response);
    }

    /// <summary>
    /// Get a specific todo item
    /// </summary>
    /// <param name="id">Todo ID</param>
    /// <returns>Todo item</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoResponse>> GetTodo(Guid id)
    {
        var todo = await _todoRepository.GetByIdAsync(id);
        if (todo == null)
        {
            return NotFound(new { message = "Todo not found" });
        }

        var response = new TodoResponse
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            IsCompleted = todo.IsCompleted,
            CreatedAt = todo.CreatedAt,
            CompletedAt = todo.CompletedAt,
            UserId = todo.UserId
        };

        return Ok(response);
    }

    /// <summary>
    /// Mark a todo as complete
    /// </summary>
    /// <param name="id">Todo ID</param>
    /// <param name="request">Update request</param>
    /// <returns>Updated todo</returns>
    [HttpPatch("{id}")]
    public async Task<ActionResult<TodoResponse>> UpdateTodo(Guid id, UpdateTodoRequest request)
    {
        var todo = await _todoRepository.GetByIdAsync(id);
        if (todo == null)
        {
            return NotFound(new { message = "Todo not found" });
        }

        todo.IsCompleted = request.IsCompleted;
        if (request.IsCompleted && todo.CompletedAt == null)
        {
            todo.CompletedAt = DateTime.UtcNow;
        }
        else if (!request.IsCompleted)
        {
            todo.CompletedAt = null;
        }

        var updatedTodo = await _todoRepository.UpdateAsync(todo);

        var response = new TodoResponse
        {
            Id = updatedTodo.Id,
            Title = updatedTodo.Title,
            Description = updatedTodo.Description,
            IsCompleted = updatedTodo.IsCompleted,
            CreatedAt = updatedTodo.CreatedAt,
            CompletedAt = updatedTodo.CompletedAt,
            UserId = updatedTodo.UserId
        };

        return Ok(response);
    }

    /// <summary>
    /// Delete a todo item
    /// </summary>
    /// <param name="id">Todo ID</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(Guid id)
    {
        var success = await _todoRepository.DeleteAsync(id);
        if (!success)
        {
            return NotFound(new { message = "Todo not found" });
        }

        return NoContent();
    }
}