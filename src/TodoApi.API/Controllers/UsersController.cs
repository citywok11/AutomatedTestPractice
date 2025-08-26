using Microsoft.AspNetCore.Mvc;
using TodoApi.Core.Interfaces;
using TodoApi.API.DTOs;

namespace TodoApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ITodoService _todoService;

    public UsersController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpPost]
    public async Task<ActionResult<UserResponse>> CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            var user = await _todoService.CreateUserAsync(request.Name, request.Email);
            var response = new UserResponse(user.Id, user.Name, user.Email, user.CreatedAt);
            return CreatedAtAction(nameof(GetUserTodos), new { userId = user.Id }, response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{userId}/todos")]
    public async Task<ActionResult<List<TodoResponse>>> GetUserTodos(int userId)
    {
        try
        {
            var todos = await _todoService.GetUserTodosAsync(userId);
            var response = todos.Select(t => new TodoResponse(t.Id, t.Title, t.Description, t.IsCompleted, t.CreatedAt, t.UserId)).ToList();
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
}