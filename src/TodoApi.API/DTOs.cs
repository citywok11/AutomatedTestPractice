namespace TodoApi.API.DTOs;

public record CreateUserRequest(string Name, string Email);
public record UserResponse(int Id, string Name, string Email, DateTime CreatedAt);

public record CreateTodoRequest(string Title, string Description);
public record TodoResponse(int Id, string Title, string Description, bool IsCompleted, DateTime CreatedAt, int UserId);

public record UpdateTodoRequest(bool IsCompleted);