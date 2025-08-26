using Microsoft.EntityFrameworkCore;
using TodoApi.Core.Entities;
using TodoApi.Core.Interfaces;
using TodoApi.Infrastructure.Data;

namespace TodoApi.Infrastructure.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly TodoDbContext _context;

    public TodoRepository(TodoDbContext context)
    {
        _context = context;
    }

    public async Task<TodoItem?> GetByIdAsync(Guid id)
    {
        return await _context.TodoItems
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<TodoItem> CreateAsync(TodoItem todo)
    {
        todo.Id = Guid.NewGuid();
        todo.CreatedAt = DateTime.UtcNow;
        
        _context.TodoItems.Add(todo);
        await _context.SaveChangesAsync();
        
        return todo;
    }

    public async Task<TodoItem> UpdateAsync(TodoItem todo)
    {
        _context.TodoItems.Update(todo);
        await _context.SaveChangesAsync();
        
        return todo;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var todo = await _context.TodoItems.FindAsync(id);
        if (todo == null)
            return false;

        _context.TodoItems.Remove(todo);
        await _context.SaveChangesAsync();
        
        return true;
    }

    public async Task<IEnumerable<TodoItem>> GetByUserIdAsync(Guid userId)
    {
        return await _context.TodoItems
            .Where(t => t.UserId == userId)
            .OrderBy(t => t.CreatedAt)
            .ToListAsync();
    }
}