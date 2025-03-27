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
}