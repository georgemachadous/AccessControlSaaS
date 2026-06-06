using System.Security.Claims;
using AccessControl.Domain.Interfaces;

namespace AccessControl.Infrastructure.Services;

public class TenantContext : ITenantContext
{
    public Guid CurrentTenantId => Guid.Empty;
    public string CurrentUserId => "anonymous";
    public string CurrentUserEmail => "anonymous";
    public bool IsAuthenticated => false;
}
