using System.ComponentModel.DataAnnotations;

namespace TodoApi.Core.Entities;

public class User
{
    public Guid Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    
    public virtual ICollection<TodoItem> Todos { get; set; } = new List<TodoItem>();
}