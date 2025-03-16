using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace UserManagement.API.Data;

public static class DbContextExtensions
{
    public static string GetCurrentUser(this DbContext context)
    {
        var httpContext = context.GetService<IHttpContextAccessor>()?.HttpContext;
        return httpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "System";
    }
} 