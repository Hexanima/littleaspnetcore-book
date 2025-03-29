namespace Little_ASP.NET_Core_Book.Models;

public class ManageUsersViewModel
{
    public AppUser[] Administrators { get; set; }
    public AppUser[] Everyone { get; set; }
}