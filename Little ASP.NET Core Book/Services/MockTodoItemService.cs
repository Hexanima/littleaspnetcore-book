using Little_ASP.NET_Core_Book.Models;

namespace Little_ASP.NET_Core_Book.Services;

public class MockTodoItemService : ITodoItemService
{
    public Task<TodoItem[]> GetIncompleteItemsAsync()
    {
        TodoItem item1 = new TodoItem
        {
            Title = "Aprender ASP.NET Core",
            DueAt = DateTimeOffset.Now.AddDays(1)
        };

        var item2 = new TodoItem { Title = "Programar Portfolio", DueAt = DateTimeOffset.Now.AddMonths(1) };

        return Task.FromResult(new[] { item1, item2 });
    }
}