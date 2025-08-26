using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using System.Text.Json;
using TodoApi.Core.DTOs;
using TodoApi.Infrastructure.Data;

namespace TodoApi.IntegrationTests;

public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly string _dbName;

    public ApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _dbName = $"TestDb_{Guid.NewGuid()}";
        
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove the existing DbContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<TodoDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                // Add a new DbContext registration with a consistent database name for this test class
                services.AddDbContext<TodoDbContext>(options =>
                    options.UseInMemoryDatabase(_dbName));
            });
        });
        
        _client = _factory.CreateClient();
    }

    private async Task<UserResponse> CreateTestUserAsync(string name = "Test User", string email = null)
    {
        email ??= $"user{Guid.NewGuid()}@example.com";
        var request = new CreateUserRequest { Name = name, Email = email };
        var response = await _client.PostAsJsonAsync("/api/users", request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<UserResponse>();
    }

    [Fact]
    public async Task CreateUser_ShouldReturnCreatedUser()
    {
        // Arrange
        var request = new CreateUserRequest
        {
            Name = "John Doe",
            Email = $"john{Guid.NewGuid()}@example.com"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/users", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var user = await response.Content.ReadFromJsonAsync<UserResponse>();
        
        Assert.NotNull(user);
        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal("John Doe", user.Name);
        Assert.Equal(request.Email, user.Email);
    }

    [Fact]
    public async Task CreateUser_WithDuplicateEmail_ShouldReturnConflict()
    {
        // Arrange
        var email = $"duplicate{Guid.NewGuid()}@example.com";
        var request = new CreateUserRequest
        {
            Name = "John Doe",
            Email = email
        };

        // Act
        await _client.PostAsJsonAsync("/api/users", request);
        var duplicateResponse = await _client.PostAsJsonAsync("/api/users", request);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Conflict, duplicateResponse.StatusCode);
    }

    [Fact]
    public async Task CreateTodo_ShouldReturnCreatedTodo()
    {
        // Arrange - Create user first
        var user = await CreateTestUserAsync("Jane Doe");

        var todoRequest = new CreateTodoRequest
        {
            Title = "Test Todo",
            Description = "Test Description",
            UserId = user.Id
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/todos", todoRequest);

        // Assert
        response.EnsureSuccessStatusCode();
        var todo = await response.Content.ReadFromJsonAsync<TodoResponse>();
        
        Assert.NotNull(todo);
        Assert.NotEqual(Guid.Empty, todo.Id);
        Assert.Equal("Test Todo", todo.Title);
        Assert.Equal("Test Description", todo.Description);
        Assert.Equal(user.Id, todo.UserId);
        Assert.False(todo.IsCompleted);
    }

    [Fact]
    public async Task CreateTodo_WithInvalidUser_ShouldReturnBadRequest()
    {
        // Arrange
        var todoRequest = new CreateTodoRequest
        {
            Title = "Test Todo",
            Description = "Test Description",
            UserId = Guid.NewGuid() // Non-existent user
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/todos", todoRequest);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetUserTodos_ShouldReturnUserTodos()
    {
        // Arrange - Create user and todos
        var user = await CreateTestUserAsync("Todo User");

        var todo1Request = new CreateTodoRequest
        {
            Title = "Todo 1",
            UserId = user.Id
        };
        var todo2Request = new CreateTodoRequest
        {
            Title = "Todo 2",
            UserId = user.Id
        };

        await _client.PostAsJsonAsync("/api/todos", todo1Request);
        await _client.PostAsJsonAsync("/api/todos", todo2Request);

        // Act
        var response = await _client.GetAsync($"/api/users/{user.Id}/todos");

        // Assert
        response.EnsureSuccessStatusCode();
        var todos = await response.Content.ReadFromJsonAsync<TodoResponse[]>();
        
        Assert.NotNull(todos);
        Assert.Equal(2, todos.Length);
    }

    [Fact]
    public async Task UpdateTodo_ShouldMarkAsComplete()
    {
        // Arrange - Create user and todo
        var user = await CreateTestUserAsync("Update User");

        var todoRequest = new CreateTodoRequest
        {
            Title = "Todo to Complete",
            UserId = user.Id
        };
        var todoResponse = await _client.PostAsJsonAsync("/api/todos", todoRequest);
        todoResponse.EnsureSuccessStatusCode();
        var todo = await todoResponse.Content.ReadFromJsonAsync<TodoResponse>();

        var updateRequest = new UpdateTodoRequest
        {
            IsCompleted = true
        };

        // Act
        var response = await _client.PatchAsJsonAsync($"/api/todos/{todo!.Id}", updateRequest);

        // Assert
        response.EnsureSuccessStatusCode();
        var updatedTodo = await response.Content.ReadFromJsonAsync<TodoResponse>();
        
        Assert.NotNull(updatedTodo);
        Assert.True(updatedTodo.IsCompleted);
        Assert.NotNull(updatedTodo.CompletedAt);
    }

    [Fact]
    public async Task DeleteTodo_ShouldReturnNoContent()
    {
        // Arrange - Create user and todo
        var user = await CreateTestUserAsync("Delete User");

        var todoRequest = new CreateTodoRequest
        {
            Title = "Todo to Delete",
            UserId = user.Id
        };
        var todoResponse = await _client.PostAsJsonAsync("/api/todos", todoRequest);
        todoResponse.EnsureSuccessStatusCode();
        var todo = await todoResponse.Content.ReadFromJsonAsync<TodoResponse>();

        // Act
        var response = await _client.DeleteAsync($"/api/todos/{todo!.Id}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);

        // Verify todo is deleted
        var getResponse = await _client.GetAsync($"/api/todos/{todo.Id}");
        Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteTodo_WithInvalidId_ShouldReturnNotFound()
    {
        // Act
        var response = await _client.DeleteAsync($"/api/todos/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }
}