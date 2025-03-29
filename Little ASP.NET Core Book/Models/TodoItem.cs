using System.ComponentModel.DataAnnotations;

namespace Little_ASP.NET_Core_Book.Models;

public class TodoItem
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public bool IsDone { get; set; }

    [Required] public string Title { get; set; }

    public DateTimeOffset? DueAt { get; set; }
}