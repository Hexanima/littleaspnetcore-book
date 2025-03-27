using Little_ASP.NET_Core_Book.Models;

namespace Little_ASP.NET_Core_Book.Services;

public interface ITodoItemService
{
    Task<TodoItem[]> GetIncompleteItemsAsync();
}