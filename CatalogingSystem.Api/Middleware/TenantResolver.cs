using System.Security.Claims;
using CatalogingSystem.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace CatalogingSystem.Api.Middleware;

public class TenantResolver
{
    private readonly RequestDelegate _next;

    public TenantResolver(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ICurrentTenantService tenantService)
    {
        StringValues tenantIdFromHeader;
        string? tenantId = null;

        if (context.Request.Path.StartsWithSegments("/Tenants") || 
            context.Request.Path.StartsWithSegments("/SuperDirector") ||
            context.Request.Path.Value.EndsWith("/Auth/super-login"))
        {
            await _next(context);
            return;
        }

        if (context.Request.Path.StartsWithSegments("/Auth"))
        {
            if (context.Request.Headers.TryGetValue("tenant", out tenantIdFromHeader))
            {
                tenantId = tenantIdFromHeader.ToString();
                try
                {
                    await tenantService.SetTenantAsync(tenantId);
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync($"Error setting tenant: {ex.Message}");
                    return;
                }
            }
            else
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Header 'tenant' is required for authentication.");
                return;
            }

            await _next(context);
            return;
        }

        string? tenantIdFromToken = null;
        if (context.User.Identity?.IsAuthenticated == true)
        {
            tenantIdFromToken = context.User.FindFirst("tenantId")?.Value;
        }

        if (context.Request.Headers.TryGetValue("tenant", out tenantIdFromHeader))
        {
            tenantId = tenantIdFromHeader.ToString();
        }
        else
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Header 'tenant' is required for this operation.");
            return;
        }

        if (tenantIdFromToken != null && tenantIdFromToken != tenantId)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Tenant in token does not match tenant in header.");
            return;
        }

        try
        {
            await tenantService.SetTenantAsync(tenantId);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync($"Error setting tenant: {ex.Message}");
            return;
        }

        await _next(context);
    }
}