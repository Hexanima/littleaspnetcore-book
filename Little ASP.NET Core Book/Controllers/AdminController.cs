using Little_ASP.NET_Core_Book.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Little_ASP.NET_Core_Book.Controllers;

[Authorize(Roles = Constants.AdministratorRole)]
public class AdminController : Controller
{
    private readonly UserManager<AppUser> _userManager;

    public AdminController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Users()
    {
        AppUser[] admins = (await _userManager.GetUsersInRoleAsync("Administrator")).ToArray();

        AppUser[] everyone = await _userManager.Users.ToArrayAsync();

        ManageUsersViewModel model = new ManageUsersViewModel()
        {
            Administrators = admins,
            Everyone = everyone
        };

        return View(model);
    }

    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveUser(Guid id)
    {
        if (id == Guid.Empty) return RedirectToAction("Users");

        AppUser? currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null) return Challenge();

        AppUser? userToDelete = await _userManager.Users.Where(user => user.Id == id).SingleOrDefaultAsync();
        if (userToDelete == null) return BadRequest("No user found.");
        if (await _userManager.IsInRoleAsync(userToDelete, Constants.AdministratorRole))
            return BadRequest("Can't delete an Administrator user.");
        bool successful = (await _userManager.DeleteAsync(userToDelete)).Succeeded;

        if (!successful) return BadRequest("Failed to delete user.");
        return RedirectToAction("Users");
    }
}