using System.ComponentModel.DataAnnotations;

namespace TodoApi.Core.Entities;

public class TodoItem
{
    public Guid Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    public bool IsCompleted { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? CompletedAt { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    public virtual User User { get; set; } = null!;
}