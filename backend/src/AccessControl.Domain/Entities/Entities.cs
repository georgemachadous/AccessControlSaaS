namespace AccessControl.Domain.Entities;

public class Company
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Document { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public ICollection<Branch> Branches { get; set; } = new List<Branch>();
    public ICollection<ApplicationEntity> Applications { get; set; } = new List<ApplicationEntity>();
    public ICollection<User> Users { get; set; } = new List<User>();
}

public class Branch
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = null!;
}

public class ApplicationEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Url { get; set; }
    public string? Icon { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid CompanyId { get; set; }
    public Company Company { get; set; } = null!;
    public ICollection<Functionality> Functionalities { get; set; } = new List<Functionality>();
    public ICollection<Role> Roles { get; set; } = new List<Role>();
    public ICollection<UserApplicationRole> UserApplicationRoles { get; set; } = new List<UserApplicationRole>();
}

public class Functionality
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Code { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid ApplicationId { get; set; }
    public ApplicationEntity Application { get; set; } = null!;
    public ICollection<RoleFunctionality> RoleFunctionalities { get; set; } = new List<RoleFunctionality>();
}

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid ApplicationId { get; set; }
    public ApplicationEntity Application { get; set; } = null!;
    public ICollection<RoleFunctionality> RoleFunctionalities { get; set; } = new List<RoleFunctionality>();
    public ICollection<UserApplicationRole> UserApplicationRoles { get; set; } = new List<UserApplicationRole>();
}

public class RoleFunctionality
{
    public Guid Id { get; set; }
    public bool CanCreate { get; set; }
    public bool CanRead { get; set; }
    public bool CanUpdate { get; set; }
    public bool CanDelete { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public Guid FunctionalityId { get; set; }
    public Functionality Functionality { get; set; } = null!;
}

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserStatus Status { get; set; } = UserStatus.Active;
    public bool IsSuperAdmin { get; set; }
    public string? SsoProvider { get; set; }
    public string? SsoId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    public Guid? CompanyId { get; set; }
    public Company? Company { get; set; }
    public ICollection<UserApplicationRole> UserApplicationRoles { get; set; } = new List<UserApplicationRole>();
    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}

public class UserApplicationRole
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid ApplicationId { get; set; }
    public ApplicationEntity Application { get; set; } = null!;
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
}

public class Permission
{
    public Guid Id { get; set; }
    public string Resource { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public bool IsAllowed { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}

public class AuditLog
{
    public Guid Id { get; set; }
    public string EntityType { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public LogAction Action { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? PerformedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
