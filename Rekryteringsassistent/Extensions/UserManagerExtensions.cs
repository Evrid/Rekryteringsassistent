using Microsoft.AspNetCore.Identity;
using Rekryteringsassistent.Models;

namespace Rekryteringsassistent.Extensions;
public static class UserManagerExtensions
{
    public static Task<ApplicationUser?> FindByIdAsync(this UserManager<ApplicationUser> manager, int userId)
    {
        var user = manager.FindByIdAsync(userId.ToString());
        return user;
    }
}

