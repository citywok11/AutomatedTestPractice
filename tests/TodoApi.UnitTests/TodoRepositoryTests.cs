using Microsoft.EntityFrameworkCore;
using TodoApi.Core.Entities;
using TodoApi.Infrastructure.Data;
using TodoApi.Infrastructure.Repositories;

namespace TodoApi.UnitTests;

public class TodoRepositoryTests
{
    private TodoDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new TodoDbContext(options);
    }

    private async Task<User> CreateTestUser(TodoDbContext context)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Test User",
            Email = "test@example.com",
            CreatedAt = DateTime.UtcNow
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateTodo_AndReturnWithId()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var user = await CreateTestUser(context);
        var repository = new TodoRepository(context);
        var todo = new TodoItem
        {
            Title = "Test Todo",
            Description = "Test Description",
            UserId = user.Id
        };

        // Act
        var result = await repository.CreateAsync(todo);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Test Todo", result.Title);
        Assert.Equal("Test Description", result.Description);
        Assert.Equal(user.Id, result.UserId);
        Assert.False(result.IsCompleted);
        Assert.True(result.CreatedAt > DateTime.MinValue);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTodo_WhenExists()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var user = await CreateTestUser(context);
        var repository = new TodoRepository(context);
        var todo = new TodoItem
        {
            Id = Guid.NewGuid(),
            Title = "Existing Todo",
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow
        };
        context.TodoItems.Add(todo);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(todo.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(todo.Id, result.Id);
        Assert.Equal("Existing Todo", result.Title);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateTodo()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var user = await CreateTestUser(context);
        var repository = new TodoRepository(context);
        var todo = new TodoItem
        {
            Id = Guid.NewGuid(),
            Title = "Original Title",
            UserId = user.Id,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };
        context.TodoItems.Add(todo);
        await context.SaveChangesAsync();

        // Act
        todo.IsCompleted = true;
        todo.CompletedAt = DateTime.UtcNow;
        var result = await repository.UpdateAsync(todo);

        // Assert
        Assert.True(result.IsCompleted);
        Assert.NotNull(result.CompletedAt);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteTodo_WhenExists()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var user = await CreateTestUser(context);
        var repository = new TodoRepository(context);
        var todo = new TodoItem
        {
            Id = Guid.NewGuid(),
            Title = "To Delete",
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow
        };
        context.TodoItems.Add(todo);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.DeleteAsync(todo.Id);

        // Assert
        Assert.True(result);
        var deletedTodo = await context.TodoItems.FindAsync(todo.Id);
        Assert.Null(deletedTodo);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenNotExists()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new TodoRepository(context);

        // Act
        var result = await repository.DeleteAsync(Guid.NewGuid());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetByUserIdAsync_ShouldReturnUserTodos()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var user = await CreateTestUser(context);
        var repository = new TodoRepository(context);
        
        var todos = new[]
        {
            new TodoItem { Id = Guid.NewGuid(), Title = "Todo 1", UserId = user.Id, CreatedAt = DateTime.UtcNow.AddMinutes(-2) },
            new TodoItem { Id = Guid.NewGuid(), Title = "Todo 2", UserId = user.Id, CreatedAt = DateTime.UtcNow.AddMinutes(-1) }
        };
        
        context.TodoItems.AddRange(todos);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByUserIdAsync(user.Id);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal("Todo 1", result.First().Title);
        Assert.Equal("Todo 2", result.Last().Title);
    }
}