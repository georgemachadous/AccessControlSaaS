namespace AccessControl.Application.DTOs;

using AccessControl.Domain.Entities;

// ==================== COMPANY ====================
public record CompanyDto(Guid Id, string Name, string? Document, string? Phone, string? Email, bool IsActive, DateTime CreatedAt);
public record CreateCompanyDto(string Name, string? Document, string? Phone, string? Email);
public record UpdateCompanyDto(string Name, string? Document, string? Phone, string? Email, bool IsActive);

// ==================== BRANCH ====================
public record BranchDto(Guid Id, string Name, string? Address, string? Phone, bool IsActive, DateTime CreatedAt, Guid CompanyId);
public record CreateBranchDto(string Name, string? Address, string? Phone, Guid CompanyId);
public record UpdateBranchDto(string Name, string? Address, string? Phone, bool IsActive);

// ==================== APPLICATION ====================
public record ApplicationDto(Guid Id, string Name, string? Description, string? Url, string? Icon, bool IsActive, DateTime CreatedAt, Guid CompanyId);
public record CreateApplicationDto(string Name, string? Description, string? Url, string? Icon, Guid CompanyId);
public record UpdateApplicationDto(string Name, string? Description, string? Url, string? Icon, bool IsActive);

// ==================== FUNCTIONALITY ====================
public record FunctionalityDto(Guid Id, string Name, string? Description, string? Code, bool IsActive, DateTime CreatedAt, Guid ApplicationId);
public record CreateFunctionalityDto(string Name, string? Description, string? Code, Guid ApplicationId);
public record UpdateFunctionalityDto(string Name, string? Description, string? Code, bool IsActive);

// ==================== ROLE ====================
public record RoleDto(Guid Id, string Name, string? Description, bool IsActive, DateTime CreatedAt, Guid ApplicationId);
public record CreateRoleDto(string Name, string? Description, Guid ApplicationId);
public record UpdateRoleDto(string Name, string? Description, bool IsActive);

// ==================== ROLE FUNCTIONALITY ====================
public record RoleFunctionalityDto(Guid Id, bool CanCreate, bool CanRead, bool CanUpdate, bool CanDelete, DateTime CreatedAt, Guid RoleId, Guid FunctionalityId);
public record CreateRoleFunctionalityDto(bool CanCreate, bool CanRead, bool CanUpdate, bool CanDelete, Guid RoleId, Guid FunctionalityId);
public record UpdateRoleFunctionalityDto(bool CanCreate, bool CanRead, bool CanUpdate, bool CanDelete);

// ==================== USER ====================
public record UserDto(Guid Id, string Name, string Email, UserStatus Status, bool IsSuperAdmin, string? SsoProvider, DateTime CreatedAt, DateTime? LastLoginAt, Guid? CompanyId);
public record CreateUserDto(string Name, string Email, string Password, bool IsSuperAdmin, Guid? CompanyId, string? SsoProvider, string? SsoId);
public record UpdateUserDto(string Name, string Email, UserStatus Status, bool IsSuperAdmin, Guid? CompanyId);

// ==================== USER APPLICATION ROLE ====================
public record UserApplicationRoleDto(Guid Id, DateTime CreatedAt, Guid UserId, Guid ApplicationId, Guid RoleId);
public record CreateUserApplicationRoleDto(Guid UserId, Guid ApplicationId, Guid RoleId);

// ==================== PERMISSION ====================
public record PermissionDto(Guid Id, string Resource, string Action, bool IsAllowed, DateTime CreatedAt, Guid UserId);
public record CreatePermissionDto(string Resource, string Action, bool IsAllowed, Guid UserId);
public record UpdatePermissionDto(string Resource, string Action, bool IsAllowed);

// ==================== AUDIT LOG ====================
public record AuditLogDto(Guid Id, string EntityType, string EntityId, LogAction Action, string? OldValues, string? NewValues, string? PerformedBy, DateTime CreatedAt);

// ==================== AUTH ====================
public record LoginRequest(string Email, string Password);
public record SsoLoginRequest(string Provider, string Token);
public record AuthResponse(string Token, string RefreshToken, UserDto User, List<UserApplicationClaim> Claims);
public record UserApplicationClaim(Guid ApplicationId, string ApplicationName, Guid RoleId, string RoleName, List<string> Permissions);
public record RefreshTokenRequest(string RefreshToken);

// ==================== DASHBOARD ====================
public record DashboardSummaryDto(
    int TotalCompanies,
    int TotalApplications,
    int TotalUsers,
    int TotalRoles,
    int ActiveUsers,
    int InactiveUsers
);
public record RecentActivityDto(string Description, string Type, DateTime Date);
