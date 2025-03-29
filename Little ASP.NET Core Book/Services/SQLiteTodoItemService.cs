using Little_ASP.NET_Core_Book.Data;
using Little_ASP.NET_Core_Book.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Little_ASP.NET_Core_Book.Services;

public class SQLiteTodoItemService : ITodoItemService
{
    private readonly ApplicationDbContext _context;

    public SQLiteTodoItemService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TodoItem[]> GetIncompleteItemsAsync(AppUser user) =>
        await _context.Items.Where(x => !x.IsDone && x.UserId == user.Id).ToArrayAsync();

    public async Task<bool> SaveItem(NewTodoItem newItem, AppUser user)
    {
        TodoItem item = new()
        {
            Title = newItem.Title,
            IsDone = false,
            DueAt = newItem.DueAt,
            Id = Guid.NewGuid(),
            UserId = user.Id
        };

        _context.Items.Add(item);
        int result = await _context.SaveChangesAsync(); // Devuelve cuantos items actualizó
        return result == 1; // 1 Siendo que actualizó 1 item
    }

    public async Task<bool> MarkDone(Guid id, AppUser user)
    {
        var item = await _context.Items.Where(x => x.Id == id && x.UserId == user.Id).SingleOrDefaultAsync();
        if (item == null) return false;

        item.IsDone = true;
        int saveResult = await _context.SaveChangesAsync(); // Devuelve cuantos items actualizó
        return saveResult == 1; // 1 Siendo que actualizó 1 item
    }
}