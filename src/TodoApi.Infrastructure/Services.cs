using TodoApi.Core.Entities;
using TodoApi.Core.Interfaces;

namespace TodoApi.Infrastructure.Services;

public class TodoService : ITodoService
{
    private readonly IUserRepository _userRepository;
    private readonly ITodoRepository _todoRepository;

    public TodoService(IUserRepository userRepository, ITodoRepository todoRepository)
    {
        _userRepository = userRepository;
        _todoRepository = todoRepository;
    }

    public async Task<User> CreateUserAsync(string name, string email)
    {
        // Check if user already exists
        var existingUser = await _userRepository.GetByEmailAsync(email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists");
        }

        var user = new User
        {
            Name = name,
            Email = email,
            CreatedAt = DateTime.UtcNow
        };

        return await _userRepository.CreateAsync(user);
    }

    public async Task<List<Todo>> GetUserTodosAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        return await _todoRepository.GetByUserIdAsync(userId);
    }

    public async Task<Todo> CreateTodoAsync(int userId, string title, string description)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        var todo = new Todo
        {
            Title = title,
            Description = description,
            UserId = userId,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        return await _todoRepository.CreateAsync(todo);
    }

    public async Task<Todo?> MarkTodoCompleteAsync(int todoId)
    {
        var todo = await _todoRepository.GetByIdAsync(todoId);
        if (todo == null)
        {
            return null;
        }

        todo.IsCompleted = true;
        return await _todoRepository.UpdateAsync(todo);
    }

    public async Task<bool> DeleteTodoAsync(int todoId)
    {
        return await _todoRepository.DeleteAsync(todoId);
    }
}