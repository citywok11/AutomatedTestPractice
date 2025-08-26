using Microsoft.EntityFrameworkCore;
using TodoApi.Core.Entities;
using TodoApi.Infrastructure.Data;
using TodoApi.Infrastructure.Repositories;

namespace TodoApi.UnitTests;

public class UserRepositoryTests
{
    private TodoDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new TodoDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateUser_AndReturnWithId()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new UserRepository(context);
        var user = new User
        {
            Name = "John Doe",
            Email = "john@example.com"
        };

        // Act
        var result = await repository.CreateAsync(user);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("John Doe", result.Name);
        Assert.Equal("john@example.com", result.Email);
        Assert.True(result.CreatedAt > DateTime.MinValue);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser_WhenExists()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new UserRepository(context);
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "Jane Doe",
            Email = "jane@example.com",
            CreatedAt = DateTime.UtcNow
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByIdAsync(user.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal("Jane Doe", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new UserRepository(context);

        // Act
        var result = await repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnUser_WhenExists()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var repository = new UserRepository(context);
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = "John Smith",
            Email = "john.smith@example.com",
            CreatedAt = DateTime.UtcNow
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Act
        var result = await repository.GetByEmailAsync("john.smith@example.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal("john.smith@example.com", result.Email);
    }
}