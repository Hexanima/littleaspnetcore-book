using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace Little_ASP.NET_Core_Book.Models;

public class NewTodoItem
{
    [Required] public string Title { get; set; }
    
    public DateTimeOffset? DueAt { get; set; }
}