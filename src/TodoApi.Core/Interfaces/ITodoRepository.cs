using TodoApi.Core.Entities;

namespace TodoApi.Core.Interfaces;

public interface ITodoRepository
{
    Task<TodoItem?> GetByIdAsync(Guid id);
    Task<TodoItem> CreateAsync(TodoItem todo);
    Task<TodoItem> UpdateAsync(TodoItem todo);
    Task<bool> DeleteAsync(Guid id);
    Task<IEnumerable<TodoItem>> GetByUserIdAsync(Guid userId);
}