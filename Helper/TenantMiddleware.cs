using Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Middleware;

public class TenantMiddleware
{
    private readonly string[] _excludedPaths = new[]
    {
        "/swagger",
        "/swagger/index.html",
        "/swagger/v1/swagger.json",
        "/api/tenants"
    };

    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext db)
    {
        // Skip tenant check for excluded paths
        if (_excludedPaths.Any(p => context.Request.Path.StartsWithSegments(p, StringComparison.OrdinalIgnoreCase)))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("X-Tenant-Id", out var tenantIdHeader))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Tenant header missing.");
            return;
        }

        if (!Guid.TryParse(tenantIdHeader, out var tenantId))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Invalid Tenant Id format.");
            return;
        }

        // Check tenant exists in DB
        bool exists = await db.Tenants.AnyAsync(t => t.Id == tenantId);
        if (!exists)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync("Tenant not found.");
            return;
        }

        // Store tenant in HttpContext for services/controllers
        context.Items["TenantId"] = tenantId;
        Console.WriteLine($"Middleware: TenantId = {tenantId}");

        // Call next middleware
        await _next(context);
    }
}
