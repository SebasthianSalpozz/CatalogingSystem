// CatalogingSystem.Api/Middleware/TenantResolver.cs
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
        if (context.Request.Path.StartsWithSegments("/Tenants"))
        {
            await _next(context);
            return;
        }

        if (context.Request.Headers.TryGetValue("tenant", out var tenantId))
        {
            try
            {
                await tenantService.SetTenantAsync(tenantId);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync($"Error al establecer el tenant: {ex.Message}");
                return;
            }
        }
        else
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Header 'tenant' es requerido para esta operación.");
            return;
        }

        await _next(context);
    }
}