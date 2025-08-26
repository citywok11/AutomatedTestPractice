namespace TodoApi.Core.Interfaces;

using TodoApi.Core.Entities;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User> CreateAsync(User user);
    Task<User?> GetByEmailAsync(string email);
}

public interface ITodoRepository
{
    Task<Todo?> GetByIdAsync(int id);
    Task<List<Todo>> GetByUserIdAsync(int userId);
    Task<Todo> CreateAsync(Todo todo);
    Task<Todo?> UpdateAsync(Todo todo);
    Task<bool> DeleteAsync(int id);
}

public interface ITodoService
{
    Task<User> CreateUserAsync(string name, string email);
    Task<List<Todo>> GetUserTodosAsync(int userId);
    Task<Todo> CreateTodoAsync(int userId, string title, string description);
    Task<Todo?> MarkTodoCompleteAsync(int todoId);
    Task<bool> DeleteTodoAsync(int todoId);
}