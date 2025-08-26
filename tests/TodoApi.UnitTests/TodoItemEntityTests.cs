using TodoApi.Core.Entities;

namespace TodoApi.UnitTests;

public class TodoItemEntityTests
{
    [Fact]
    public void TodoItem_Creation_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Test Todo";
        var description = "Test Description";
        var userId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;

        // Act
        var todo = new TodoItem
        {
            Id = id,
            Title = title,
            Description = description,
            UserId = userId,
            CreatedAt = createdAt,
            IsCompleted = false
        };

        // Assert
        Assert.Equal(id, todo.Id);
        Assert.Equal(title, todo.Title);
        Assert.Equal(description, todo.Description);
        Assert.Equal(userId, todo.UserId);
        Assert.Equal(createdAt, todo.CreatedAt);
        Assert.False(todo.IsCompleted);
        Assert.Null(todo.CompletedAt);
    }

    [Fact]
    public void TodoItem_MarkAsCompleted_ShouldSetCompletedAt()
    {
        // Arrange
        var todo = new TodoItem
        {
            Id = Guid.NewGuid(),
            Title = "Test Todo",
            UserId = Guid.NewGuid(),
            IsCompleted = false
        };

        // Act
        todo.IsCompleted = true;
        todo.CompletedAt = DateTime.UtcNow;

        // Assert
        Assert.True(todo.IsCompleted);
        Assert.NotNull(todo.CompletedAt);
    }
}