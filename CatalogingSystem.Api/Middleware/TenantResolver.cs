namespace CatalogingSystem.Api.Middleware;

using CatalogingSystem.Core.Interfaces;

public class TenantResolver
{
    private readonly RequestDelegate _next;

    public TenantResolver(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ICurrentTenantService tenantService)
    {
        if (context.Request.Headers.TryGetValue("tenant", out var tenantId))
        {
            await tenantService.SetTenantAsync(tenantId);
        }
        await _next(context);
    }
}