using TodoApi.Infrastructure.Services;
using TodoApi.Core.Interfaces;
using TodoApi.Core.Entities;
using Moq;

namespace TodoApi.UnitTests;

public class TodoServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ITodoRepository> _todoRepositoryMock;
    private readonly TodoService _todoService;

    public TodoServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _todoRepositoryMock = new Mock<ITodoRepository>();
        _todoService = new TodoService(_userRepositoryMock.Object, _todoRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldCreateUser_WhenEmailIsUnique()
    {
        // Arrange
        var name = "John Doe";
        var email = "john@example.com";
        _userRepositoryMock.Setup(x => x.GetByEmailAsync(email)).ReturnsAsync((User?)null);
        _userRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<User>())).ReturnsAsync((User user) => user);

        // Act
        var result = await _todoService.CreateUserAsync(name, email);

        // Assert
        Assert.Equal(name, result.Name);
        Assert.Equal(email, result.Email);
        _userRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldThrowException_WhenEmailExists()
    {
        // Arrange
        var name = "John Doe";
        var email = "john@example.com";
        var existingUser = new User { Id = 1, Name = "Jane", Email = email };
        _userRepositoryMock.Setup(x => x.GetByEmailAsync(email)).ReturnsAsync(existingUser);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _todoService.CreateUserAsync(name, email));
    }

    [Fact]
    public async Task CreateTodoAsync_ShouldCreateTodo_WhenUserExists()
    {
        // Arrange
        var userId = 1;
        var title = "Test Todo";
        var description = "Test Description";
        var user = new User { Id = userId, Name = "John", Email = "john@example.com" };
        _userRepositoryMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);
        _todoRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Todo>())).ReturnsAsync((Todo todo) => todo);

        // Act
        var result = await _todoService.CreateTodoAsync(userId, title, description);

        // Assert
        Assert.Equal(title, result.Title);
        Assert.Equal(description, result.Description);
        Assert.Equal(userId, result.UserId);
        Assert.False(result.IsCompleted);
    }

    [Fact]
    public async Task MarkTodoCompleteAsync_ShouldMarkComplete_WhenTodoExists()
    {
        // Arrange
        var todoId = 1;
        var todo = new Todo { Id = todoId, Title = "Test", IsCompleted = false };
        _todoRepositoryMock.Setup(x => x.GetByIdAsync(todoId)).ReturnsAsync(todo);
        _todoRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Todo>())).ReturnsAsync((Todo t) => t);

        // Act
        var result = await _todoService.MarkTodoCompleteAsync(todoId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsCompleted);
    }
}