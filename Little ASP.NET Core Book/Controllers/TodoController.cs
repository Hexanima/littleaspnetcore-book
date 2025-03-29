using Little_ASP.NET_Core_Book.Models;
using Little_ASP.NET_Core_Book.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Little_ASP.NET_Core_Book.Controllers;

[Authorize]
public class TodoController : Controller
{
    private readonly ITodoItemService _todoItemService;
    private readonly UserManager<AppUser> _userManager;

    // Inyección de dependencias
    public TodoController(ITodoItemService todoItemService, UserManager<AppUser> userManager)
    {
        _todoItemService = todoItemService;
        _userManager = userManager;
    }

    // IActionResult puede retornar Vistas, JSON, y estados HTTP
    public async Task<IActionResult> Index()
    {
        // Busca al usuario actual
        AppUser? currentUser = await _userManager.GetUserAsync(User);

        // Si no existe, al login
        if (currentUser == null) return Challenge();
        // TERMINAR ESTOOO

        // SELECT en DB
        TodoItem[] items = await _todoItemService.GetIncompleteItemsAsync(currentUser);

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

        AppUser? currentUser = await _userManager.GetUserAsync(User);

        if (currentUser == null) return Challenge();

        bool successful = await _todoItemService.SaveItem(newItem, currentUser);

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

        AppUser? currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null) return Challenge();

        bool successful = await _todoItemService.MarkDone(id, currentUser);
        if (!successful) return BadRequest("Could not mark as done.");

        return RedirectToAction("Index");
    }
}