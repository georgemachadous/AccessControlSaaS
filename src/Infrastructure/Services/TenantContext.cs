using System.Security.Claims;
using AccessControl.Domain.Interfaces;

namespace AccessControl.Infrastructure.Services;

public class TenantContext : ITenantContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid CurrentTenantId
    {
        get
        {
            var tenantId = _httpContextAccessor.HttpContext?.User?.FindFirst("tenant_id")?.Value;
            return tenantId is not null && Guid.TryParse(tenantId, out var id) ? id : Guid.Empty;
        }
    }

    public string CurrentUserId =>
        _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "anonymous";

    public string CurrentUserEmail =>
        _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? "anonymous";

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}
