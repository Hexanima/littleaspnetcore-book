using Little_ASP.NET_Core_Book.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Little_ASP.NET_Core_Book.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser,IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TodoItem> Items { get; set; }
}