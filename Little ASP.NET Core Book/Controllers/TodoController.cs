using Little_ASP.NET_Core_Book.Models;
using Little_ASP.NET_Core_Book.Services;
using Microsoft.AspNetCore.Mvc;

namespace Little_ASP.NET_Core_Book.Controllers;

public class TodoController : Controller
{
    private readonly ITodoItemService _todoItemService;

    public TodoController(ITodoItemService todoItemService)
    {
        _todoItemService = todoItemService;
    }

    // IActionResult puede retornar Vistas, JSON, y estados HTTP
    public async Task<IActionResult> Index()
    {
        // TERMINAR ESTOOO

        // Get a DB
        TodoItem[] items = await _todoItemService.GetIncompleteItemsAsync();

        // Formatear items
        TodoViewModel model = new TodoViewModel()
        {
            Items = items
        };

        // res.view()? EJS
        return View(model);
    }

    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddItem(NewTodoItem newItem)
    {
        if (!ModelState.IsValid) return RedirectToAction("Index");

        bool successful = await _todoItemService.SaveItem(newItem);

        if (!successful)
        {
            return BadRequest("Could not add item.");
        }

        return RedirectToAction("Index");
    }

    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkDone(Guid id)
    {
        if (id == Guid.Empty) return RedirectToAction("Index");

        bool successful = await _todoItemService.MarkDone(id);
        if (!successful) return BadRequest("Could not mark as done.");

        return RedirectToAction("Index");
    }
}