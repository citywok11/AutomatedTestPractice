using System.ComponentModel.DataAnnotations;

namespace TodoApi.Core.DTOs;

public class CreateTodoRequest
{
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
}

public class UpdateTodoRequest
{
    public bool IsCompleted { get; set; }
}

public class TodoResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Guid UserId { get; set; }
}