using Little_ASP.NET_Core_Book.Models;
using Microsoft.AspNetCore.Identity;

namespace Little_ASP.NET_Core_Book.Services;

public class MockTodoItemService : ITodoItemService
{
    public Task<TodoItem[]> GetIncompleteItemsAsync(AppUser user)
    {
        TodoItem item1 = new TodoItem
        {
            Title = "Aprender ASP.NET Core",
            DueAt = DateTimeOffset.Now.AddDays(1)
        };

        var item2 = new TodoItem { Title = "Programar Portfolio", DueAt = DateTimeOffset.Now.AddMonths(1) };

        return Task.FromResult(new[] { item1, item2 });
    }

    public Task<bool> SaveItem(NewTodoItem item, AppUser user)
    {
        throw new NotImplementedException();
    }

    public Task<bool> MarkDone(Guid id, AppUser user)
    {
        throw new NotImplementedException();
    }
}