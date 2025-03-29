using Little_ASP.NET_Core_Book.Models;
using Microsoft.AspNetCore.Identity;

namespace Little_ASP.NET_Core_Book.Services;

public interface ITodoItemService
{
    Task<TodoItem[]> GetIncompleteItemsAsync(AppUser user);

    Task<bool> SaveItem(NewTodoItem item, AppUser user);

    Task<bool> MarkDone(Guid id, AppUser user);
}