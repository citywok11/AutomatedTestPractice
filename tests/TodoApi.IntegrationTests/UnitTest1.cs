using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Text.Json;
using TodoApi.API.DTOs;

namespace TodoApi.IntegrationTests;

public class TodoApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public TodoApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CreateUser_ShouldReturnCreatedUser()
    {
        // Arrange
        var request = new CreateUserRequest("John Doe", "john@example.com");
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/users", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<UserResponse>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        Assert.NotNull(user);
        Assert.Equal("John Doe", user.Name);
        Assert.Equal("john@example.com", user.Email);
    }

    [Fact]
    public async Task GetUserTodos_ShouldReturnEmptyList_ForNewUser()
    {
        // Arrange - Create a user first
        var userRequest = new CreateUserRequest("Jane Doe", "jane@example.com");
        var userJson = JsonSerializer.Serialize(userRequest);
        var userContent = new StringContent(userJson, Encoding.UTF8, "application/json");
        var userResponse = await _client.PostAsync("/api/users", userContent);
        userResponse.EnsureSuccessStatusCode();
        
        var userResponseString = await userResponse.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<UserResponse>(userResponseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Act
        var response = await _client.GetAsync($"/api/users/{user!.Id}/todos");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var todos = JsonSerializer.Deserialize<List<TodoResponse>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        Assert.NotNull(todos);
        Assert.Empty(todos);
    }

    [Fact]
    public async Task FullTodoWorkflow_ShouldWork()
    {
        // Create user
        var userRequest = new CreateUserRequest("Test User", "test@example.com");
        var userJson = JsonSerializer.Serialize(userRequest);
        var userContent = new StringContent(userJson, Encoding.UTF8, "application/json");
        var userResponse = await _client.PostAsync("/api/users", userContent);
        userResponse.EnsureSuccessStatusCode();
        
        var userResponseString = await userResponse.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<UserResponse>(userResponseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Create todo
        var todoRequest = new CreateTodoRequest("Test Todo", "Test Description");
        var todoJson = JsonSerializer.Serialize(todoRequest);
        var todoContent = new StringContent(todoJson, Encoding.UTF8, "application/json");
        var todoResponse = await _client.PostAsync($"/api/todos?userId={user!.Id}", todoContent);
        todoResponse.EnsureSuccessStatusCode();

        var todoResponseString = await todoResponse.Content.ReadAsStringAsync();
        var todo = JsonSerializer.Deserialize<TodoResponse>(todoResponseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Verify todo was created
        Assert.NotNull(todo);
        Assert.Equal("Test Todo", todo.Title);
        Assert.False(todo.IsCompleted);

        // Mark todo complete
        var updateRequest = new UpdateTodoRequest(true);
        var updateJson = JsonSerializer.Serialize(updateRequest);
        var updateContent = new StringContent(updateJson, Encoding.UTF8, "application/json");
        var updateResponse = await _client.PatchAsync($"/api/todos/{todo.Id}", updateContent);
        updateResponse.EnsureSuccessStatusCode();

        // Delete todo
        var deleteResponse = await _client.DeleteAsync($"/api/todos/{todo.Id}");
        Assert.Equal(System.Net.HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }
}