using TodoApi.Core.Entities;

namespace TodoApi.UnitTests;

public class UserEntityTests
{
    [Fact]
    public void User_Creation_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        var name = "John Doe";
        var email = "john@example.com";
        var createdAt = DateTime.UtcNow;

        // Act
        var user = new User
        {
            Id = id,
            Name = name,
            Email = email,
            CreatedAt = createdAt
        };

        // Assert
        Assert.Equal(id, user.Id);
        Assert.Equal(name, user.Name);
        Assert.Equal(email, user.Email);
        Assert.Equal(createdAt, user.CreatedAt);
        Assert.NotNull(user.Todos);
        Assert.Empty(user.Todos);
    }

    [Fact]
    public void User_WithEmptyName_ShouldHaveEmptyStringDefault()
    {
        // Arrange & Act
        var user = new User();

        // Assert
        Assert.Equal(string.Empty, user.Name);
        Assert.Equal(string.Empty, user.Email);
    }
}