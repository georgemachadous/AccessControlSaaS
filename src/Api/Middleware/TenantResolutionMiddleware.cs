using System.Security.Claims;

namespace AccessControl.Api.Middleware;

public class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TenantResolutionMiddleware> _logger;

    public TenantResolutionMiddleware(RequestDelegate next, ILogger<TenantResolutionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            var tenantId = context.User.FindFirst("tenant_id")?.Value;
            if (tenantId is not null)
            {
                context.Items["TenantId"] = tenantId;
                _logger.LogDebug("Tenant resolved: {TenantId}", tenantId);
            }
        }
        await _next(context);
    }
}
