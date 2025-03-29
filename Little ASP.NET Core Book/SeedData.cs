using Little_ASP.NET_Core_Book.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Little_ASP.NET_Core_Book;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        await EnsureRolesAsync(roleManager);

        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        await EnsureTestAdminAsync(userManager);
    }

    private static async Task EnsureRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
    {
        bool alreadyExists = await roleManager.RoleExistsAsync(Constants.AdministratorRole);
        if (alreadyExists) return;

        await roleManager.CreateAsync(new IdentityRole<Guid>(Constants.AdministratorRole));
    }

    private static async Task EnsureTestAdminAsync(UserManager<AppUser> userManager)
    {
        const string adminEmail = "admin@todo.local";
        const string adminPassword = "Admin1234!";

        AppUser? testAdmin = await userManager.Users.Where(x => x.UserName == adminEmail).SingleOrDefaultAsync();
        if (testAdmin != null) return;

        testAdmin = new AppUser()
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };
        await userManager.CreateAsync(testAdmin, adminPassword);
        await userManager.AddToRoleAsync(testAdmin, Constants.AdministratorRole);
    }
}