using Microsoft.AspNetCore.Mvc;
using TodoApi.Core.DTOs;
using TodoApi.Core.Entities;
using TodoApi.Core.Interfaces;

namespace TodoApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ITodoRepository _todoRepository;

    public UsersController(IUserRepository userRepository, ITodoRepository todoRepository)
    {
        _userRepository = userRepository;
        _todoRepository = todoRepository;
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="request">User creation request</param>
    /// <returns>Created user</returns>
    [HttpPost]
    public async Task<ActionResult<UserResponse>> CreateUser(CreateUserRequest request)
    {
        // Check if user with email already exists
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return Conflict(new { message = "User with this email already exists" });
        }

        var user = new User
        {
            Name = request.Name,
            Email = request.Email
        };

        var createdUser = await _userRepository.CreateAsync(user);

        var response = new UserResponse
        {
            Id = createdUser.Id,
            Name = createdUser.Name,
            Email = createdUser.Email,
            CreatedAt = createdUser.CreatedAt
        };

        return CreatedAtAction(nameof(GetUserTodos), new { id = createdUser.Id }, response);
    }

    /// <summary>
    /// Get all todos for a specific user
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>List of user's todos</returns>
    [HttpGet("{id}/todos")]
    public async Task<ActionResult<IEnumerable<TodoResponse>>> GetUserTodos(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        var todos = await _todoRepository.GetByUserIdAsync(id);
        
        var response = todos.Select(t => new TodoResponse
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            IsCompleted = t.IsCompleted,
            CreatedAt = t.CreatedAt,
            CompletedAt = t.CompletedAt,
            UserId = t.UserId
        });

        return Ok(response);
    }
}