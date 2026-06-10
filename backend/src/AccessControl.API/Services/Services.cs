using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AccessControl.Application.DTOs;
using AccessControl.Domain.Entities;
using AccessControl.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AccessControl.API.Services;

// ==================== INTERFACES ====================
public interface ICompanyService { Task<IEnumerable<CompanyDto>> GetAllAsync(); Task<CompanyDto?> GetByIdAsync(Guid id); Task<CompanyDto> CreateAsync(CreateCompanyDto dto); Task<CompanyDto?> UpdateAsync(Guid id, UpdateCompanyDto dto); Task<bool> DeleteAsync(Guid id); }
public interface IBranchService { Task<IEnumerable<BranchDto>> GetAllAsync(); Task<BranchDto?> GetByIdAsync(Guid id); Task<BranchDto> CreateAsync(CreateBranchDto dto); Task<BranchDto?> UpdateAsync(Guid id, UpdateBranchDto dto); Task<bool> DeleteAsync(Guid id); }
public interface IApplicationService { Task<IEnumerable<ApplicationDto>> GetAllAsync(); Task<ApplicationDto?> GetByIdAsync(Guid id); Task<ApplicationDto> CreateAsync(CreateApplicationDto dto); Task<ApplicationDto?> UpdateAsync(Guid id, UpdateApplicationDto dto); Task<bool> DeleteAsync(Guid id); }
public interface IFunctionalityService { Task<IEnumerable<FunctionalityDto>> GetAllAsync(); Task<FunctionalityDto?> GetByIdAsync(Guid id); Task<FunctionalityDto> CreateAsync(CreateFunctionalityDto dto); Task<FunctionalityDto?> UpdateAsync(Guid id, UpdateFunctionalityDto dto); Task<bool> DeleteAsync(Guid id); }
public interface IRoleService { Task<IEnumerable<RoleDto>> GetAllAsync(); Task<RoleDto?> GetByIdAsync(Guid id); Task<RoleDto> CreateAsync(CreateRoleDto dto); Task<RoleDto?> UpdateAsync(Guid id, UpdateRoleDto dto); Task<bool> DeleteAsync(Guid id); }
public interface IRoleFunctionalityService { Task<IEnumerable<RoleFunctionalityDto>> GetAllAsync(); Task<RoleFunctionalityDto?> GetByIdAsync(Guid id); Task<RoleFunctionalityDto> CreateAsync(CreateRoleFunctionalityDto dto); Task<RoleFunctionalityDto?> UpdateAsync(Guid id, UpdateRoleFunctionalityDto dto); Task<bool> DeleteAsync(Guid id); }
public interface IUserService { Task<IEnumerable<UserDto>> GetAllAsync(); Task<UserDto?> GetByIdAsync(Guid id); Task<UserDto> CreateAsync(CreateUserDto dto); Task<UserDto?> UpdateAsync(Guid id, UpdateUserDto dto); Task<bool> DeleteAsync(Guid id); }
public interface IUserApplicationRoleService { Task<IEnumerable<UserApplicationRoleDto>> GetAllAsync(); Task<UserApplicationRoleDto?> GetByIdAsync(Guid id); Task<UserApplicationRoleDto> CreateAsync(CreateUserApplicationRoleDto dto); Task<bool> DeleteAsync(Guid id); }
public interface IPermissionService { Task<IEnumerable<PermissionDto>> GetAllAsync(); Task<PermissionDto?> GetByIdAsync(Guid id); Task<PermissionDto> CreateAsync(CreatePermissionDto dto); Task<PermissionDto?> UpdateAsync(Guid id, UpdatePermissionDto dto); Task<bool> DeleteAsync(Guid id); }
public interface IAuthService { Task<AuthResponse?> LoginAsync(LoginRequest request); Task<AuthResponse?> SsoLoginAsync(SsoLoginRequest request); Task<AuthResponse?> RefreshTokenAsync(RefreshTokenRequest request); }
public interface IDashboardService { Task<DashboardSummaryDto> GetSummaryAsync(); Task<IEnumerable<RecentActivityDto>> GetRecentActivityAsync(int count); }

// ==================== COMPANY SERVICE ====================
public class CompanyService : ICompanyService
{
    private readonly AppDbContext _ctx;
    public CompanyService(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<CompanyDto>> GetAllAsync() =>
        await _ctx.Companies.Select(c => new CompanyDto(c.Id, c.Name, c.Document, c.Phone, c.Email, c.IsActive, c.CreatedAt)).ToListAsync();

    public async Task<CompanyDto?> GetByIdAsync(Guid id)
    {
        var c = await _ctx.Companies.FindAsync(id);
        return c == null ? null : new CompanyDto(c.Id, c.Name, c.Document, c.Phone, c.Email, c.IsActive, c.CreatedAt);
    }

    public async Task<CompanyDto> CreateAsync(CreateCompanyDto dto)
    {
        var c = new Company { Id = Guid.NewGuid(), Name = dto.Name, Document = dto.Document, Phone = dto.Phone, Email = dto.Email };
        _ctx.Companies.Add(c); await _ctx.SaveChangesAsync();
        return new CompanyDto(c.Id, c.Name, c.Document, c.Phone, c.Email, c.IsActive, c.CreatedAt);
    }

    public async Task<CompanyDto?> UpdateAsync(Guid id, UpdateCompanyDto dto)
    {
        var c = await _ctx.Companies.FindAsync(id);
        if (c == null) return null;
        c.Name = dto.Name; c.Document = dto.Document; c.Phone = dto.Phone; c.Email = dto.Email; c.IsActive = dto.IsActive; c.UpdatedAt = DateTime.UtcNow;
        await _ctx.SaveChangesAsync();
        return new CompanyDto(c.Id, c.Name, c.Document, c.Phone, c.Email, c.IsActive, c.CreatedAt);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var c = await _ctx.Companies.FindAsync(id);
        if (c == null) return false;
        _ctx.Companies.Remove(c); await _ctx.SaveChangesAsync(); return true;
    }
}

// ==================== BRANCH SERVICE ====================
public class BranchService : IBranchService
{
    private readonly AppDbContext _ctx;
    public BranchService(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<BranchDto>> GetAllAsync() =>
        await _ctx.Branches.Select(b => new BranchDto(b.Id, b.Name, b.Address, b.Phone, b.IsActive, b.CreatedAt, b.CompanyId)).ToListAsync();

    public async Task<BranchDto?> GetByIdAsync(Guid id)
    {
        var b = await _ctx.Branches.FindAsync(id);
        return b == null ? null : new BranchDto(b.Id, b.Name, b.Address, b.Phone, b.IsActive, b.CreatedAt, b.CompanyId);
    }

    public async Task<BranchDto> CreateAsync(CreateBranchDto dto)
    {
        var b = new Branch { Id = Guid.NewGuid(), Name = dto.Name, Address = dto.Address, Phone = dto.Phone, CompanyId = dto.CompanyId };
        _ctx.Branches.Add(b); await _ctx.SaveChangesAsync();
        return new BranchDto(b.Id, b.Name, b.Address, b.Phone, b.IsActive, b.CreatedAt, b.CompanyId);
    }

    public async Task<BranchDto?> UpdateAsync(Guid id, UpdateBranchDto dto)
    {
        var b = await _ctx.Branches.FindAsync(id);
        if (b == null) return null;
        b.Name = dto.Name; b.Address = dto.Address; b.Phone = dto.Phone; b.IsActive = dto.IsActive;
        await _ctx.SaveChangesAsync();
        return new BranchDto(b.Id, b.Name, b.Address, b.Phone, b.IsActive, b.CreatedAt, b.CompanyId);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var b = await _ctx.Branches.FindAsync(id);
        if (b == null) return false;
        _ctx.Branches.Remove(b); await _ctx.SaveChangesAsync(); return true;
    }
}

// ==================== APPLICATION SERVICE ====================
public class ApplicationService : IApplicationService
{
    private readonly AppDbContext _ctx;
    public ApplicationService(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<ApplicationDto>> GetAllAsync() =>
        await _ctx.Applications.Select(a => new ApplicationDto(a.Id, a.Name, a.Description, a.Url, a.Icon, a.IsActive, a.CreatedAt, a.CompanyId)).ToListAsync();

    public async Task<ApplicationDto?> GetByIdAsync(Guid id)
    {
        var a = await _ctx.Applications.FindAsync(id);
        return a == null ? null : new ApplicationDto(a.Id, a.Name, a.Description, a.Url, a.Icon, a.IsActive, a.CreatedAt, a.CompanyId);
    }

    public async Task<ApplicationDto> CreateAsync(CreateApplicationDto dto)
    {
        var a = new ApplicationEntity { Id = Guid.NewGuid(), Name = dto.Name, Description = dto.Description, Url = dto.Url, Icon = dto.Icon, CompanyId = dto.CompanyId };
        _ctx.Applications.Add(a); await _ctx.SaveChangesAsync();
        return new ApplicationDto(a.Id, a.Name, a.Description, a.Url, a.Icon, a.IsActive, a.CreatedAt, a.CompanyId);
    }

    public async Task<ApplicationDto?> UpdateAsync(Guid id, UpdateApplicationDto dto)
    {
        var a = await _ctx.Applications.FindAsync(id);
        if (a == null) return null;
        a.Name = dto.Name; a.Description = dto.Description; a.Url = dto.Url; a.Icon = dto.Icon; a.IsActive = dto.IsActive;
        await _ctx.SaveChangesAsync();
        return new ApplicationDto(a.Id, a.Name, a.Description, a.Url, a.Icon, a.IsActive, a.CreatedAt, a.CompanyId);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var a = await _ctx.Applications.FindAsync(id);
        if (a == null) return false;
        _ctx.Applications.Remove(a); await _ctx.SaveChangesAsync(); return true;
    }
}

// ==================== FUNCTIONALITY SERVICE ====================
public class FunctionalityService : IFunctionalityService
{
    private readonly AppDbContext _ctx;
    public FunctionalityService(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<FunctionalityDto>> GetAllAsync() =>
        await _ctx.Functionalities.Select(f => new FunctionalityDto(f.Id, f.Name, f.Description, f.Code, f.IsActive, f.CreatedAt, f.ApplicationId)).ToListAsync();

    public async Task<FunctionalityDto?> GetByIdAsync(Guid id)
    {
        var f = await _ctx.Functionalities.FindAsync(id);
        return f == null ? null : new FunctionalityDto(f.Id, f.Name, f.Description, f.Code, f.IsActive, f.CreatedAt, f.ApplicationId);
    }

    public async Task<FunctionalityDto> CreateAsync(CreateFunctionalityDto dto)
    {
        var f = new Functionality { Id = Guid.NewGuid(), Name = dto.Name, Description = dto.Description, Code = dto.Code, ApplicationId = dto.ApplicationId };
        _ctx.Functionalities.Add(f); await _ctx.SaveChangesAsync();
        return new FunctionalityDto(f.Id, f.Name, f.Description, f.Code, f.IsActive, f.CreatedAt, f.ApplicationId);
    }

    public async Task<FunctionalityDto?> UpdateAsync(Guid id, UpdateFunctionalityDto dto)
    {
        var f = await _ctx.Functionalities.FindAsync(id);
        if (f == null) return null;
        f.Name = dto.Name; f.Description = dto.Description; f.Code = dto.Code; f.IsActive = dto.IsActive;
        await _ctx.SaveChangesAsync();
        return new FunctionalityDto(f.Id, f.Name, f.Description, f.Code, f.IsActive, f.CreatedAt, f.ApplicationId);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var f = await _ctx.Functionalities.FindAsync(id);
        if (f == null) return false;
        _ctx.Functionalities.Remove(f); await _ctx.SaveChangesAsync(); return true;
    }
}

// ==================== ROLE SERVICE ====================
public class RoleService : IRoleService
{
    private readonly AppDbContext _ctx;
    public RoleService(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<RoleDto>> GetAllAsync() =>
        await _ctx.Roles.Select(r => new RoleDto(r.Id, r.Name, r.Description, r.IsActive, r.CreatedAt, r.ApplicationId)).ToListAsync();

    public async Task<RoleDto?> GetByIdAsync(Guid id)
    {
        var r = await _ctx.Roles.FindAsync(id);
        return r == null ? null : new RoleDto(r.Id, r.Name, r.Description, r.IsActive, r.CreatedAt, r.ApplicationId);
    }

    public async Task<RoleDto> CreateAsync(CreateRoleDto dto)
    {
        var r = new Role { Id = Guid.NewGuid(), Name = dto.Name, Description = dto.Description, ApplicationId = dto.ApplicationId };
        _ctx.Roles.Add(r); await _ctx.SaveChangesAsync();
        return new RoleDto(r.Id, r.Name, r.Description, r.IsActive, r.CreatedAt, r.ApplicationId);
    }

    public async Task<RoleDto?> UpdateAsync(Guid id, UpdateRoleDto dto)
    {
        var r = await _ctx.Roles.FindAsync(id);
        if (r == null) return null;
        r.Name = dto.Name; r.Description = dto.Description; r.IsActive = dto.IsActive;
        await _ctx.SaveChangesAsync();
        return new RoleDto(r.Id, r.Name, r.Description, r.IsActive, r.CreatedAt, r.ApplicationId);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var r = await _ctx.Roles.FindAsync(id);
        if (r == null) return false;
        _ctx.Roles.Remove(r); await _ctx.SaveChangesAsync(); return true;
    }
}

// ==================== ROLE FUNCTIONALITY SERVICE ====================
public class RoleFunctionalityService : IRoleFunctionalityService
{
    private readonly AppDbContext _ctx;
    public RoleFunctionalityService(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<RoleFunctionalityDto>> GetAllAsync() =>
        await _ctx.RoleFunctionalities.Select(rf => new RoleFunctionalityDto(rf.Id, rf.CanCreate, rf.CanRead, rf.CanUpdate, rf.CanDelete, rf.CreatedAt, rf.RoleId, rf.FunctionalityId)).ToListAsync();

    public async Task<RoleFunctionalityDto?> GetByIdAsync(Guid id)
    {
        var rf = await _ctx.RoleFunctionalities.FindAsync(id);
        return rf == null ? null : new RoleFunctionalityDto(rf.Id, rf.CanCreate, rf.CanRead, rf.CanUpdate, rf.CanDelete, rf.CreatedAt, rf.RoleId, rf.FunctionalityId);
    }

    public async Task<RoleFunctionalityDto> CreateAsync(CreateRoleFunctionalityDto dto)
    {
        var rf = new RoleFunctionality { Id = Guid.NewGuid(), CanCreate = dto.CanCreate, CanRead = dto.CanRead, CanUpdate = dto.CanUpdate, CanDelete = dto.CanDelete, RoleId = dto.RoleId, FunctionalityId = dto.FunctionalityId };
        _ctx.RoleFunctionalities.Add(rf); await _ctx.SaveChangesAsync();
        return new RoleFunctionalityDto(rf.Id, rf.CanCreate, rf.CanRead, rf.CanUpdate, rf.CanDelete, rf.CreatedAt, rf.RoleId, rf.FunctionalityId);
    }

    public async Task<RoleFunctionalityDto?> UpdateAsync(Guid id, UpdateRoleFunctionalityDto dto)
    {
        var rf = await _ctx.RoleFunctionalities.FindAsync(id);
        if (rf == null) return null;
        rf.CanCreate = dto.CanCreate; rf.CanRead = dto.CanRead; rf.CanUpdate = dto.CanUpdate; rf.CanDelete = dto.CanDelete;
        await _ctx.SaveChangesAsync();
        return new RoleFunctionalityDto(rf.Id, rf.CanCreate, rf.CanRead, rf.CanUpdate, rf.CanDelete, rf.CreatedAt, rf.RoleId, rf.FunctionalityId);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var rf = await _ctx.RoleFunctionalities.FindAsync(id);
        if (rf == null) return false;
        _ctx.RoleFunctionalities.Remove(rf); await _ctx.SaveChangesAsync(); return true;
    }
}

// ==================== USER SERVICE ====================
public class UserService : IUserService
{
    private readonly AppDbContext _ctx;
    public UserService(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<UserDto>> GetAllAsync() =>
        await _ctx.Users.Select(u => new UserDto(u.Id, u.Name, u.Email, u.Status, u.IsSuperAdmin, u.SsoProvider, u.CreatedAt, u.LastLoginAt, u.CompanyId)).ToListAsync();

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var u = await _ctx.Users.FindAsync(id);
        return u == null ? null : new UserDto(u.Id, u.Name, u.Email, u.Status, u.IsSuperAdmin, u.SsoProvider, u.CreatedAt, u.LastLoginAt, u.CompanyId);
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        var u = new User
        {
            Id = Guid.NewGuid(), Name = dto.Name, Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            IsSuperAdmin = dto.IsSuperAdmin, CompanyId = dto.CompanyId,
            SsoProvider = dto.SsoProvider, SsoId = dto.SsoId
        };
        _ctx.Users.Add(u); await _ctx.SaveChangesAsync();
        return new UserDto(u.Id, u.Name, u.Email, u.Status, u.IsSuperAdmin, u.SsoProvider, u.CreatedAt, u.LastLoginAt, u.CompanyId);
    }

    public async Task<UserDto?> UpdateAsync(Guid id, UpdateUserDto dto)
    {
        var u = await _ctx.Users.FindAsync(id);
        if (u == null) return null;
        u.Name = dto.Name; u.Email = dto.Email; u.Status = dto.Status; u.IsSuperAdmin = dto.IsSuperAdmin; u.CompanyId = dto.CompanyId;
        await _ctx.SaveChangesAsync();
        return new UserDto(u.Id, u.Name, u.Email, u.Status, u.IsSuperAdmin, u.SsoProvider, u.CreatedAt, u.LastLoginAt, u.CompanyId);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var u = await _ctx.Users.FindAsync(id);
        if (u == null) return false;
        _ctx.Users.Remove(u); await _ctx.SaveChangesAsync(); return true;
    }
}

// ==================== USER APPLICATION ROLE SERVICE ====================
public class UserApplicationRoleService : IUserApplicationRoleService
{
    private readonly AppDbContext _ctx;
    public UserApplicationRoleService(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<UserApplicationRoleDto>> GetAllAsync() =>
        await _ctx.UserApplicationRoles.Select(ur => new UserApplicationRoleDto(ur.Id, ur.CreatedAt, ur.UserId, ur.ApplicationId, ur.RoleId)).ToListAsync();

    public async Task<UserApplicationRoleDto?> GetByIdAsync(Guid id)
    {
        var ur = await _ctx.UserApplicationRoles.FindAsync(id);
        return ur == null ? null : new UserApplicationRoleDto(ur.Id, ur.CreatedAt, ur.UserId, ur.ApplicationId, ur.RoleId);
    }

    public async Task<UserApplicationRoleDto> CreateAsync(CreateUserApplicationRoleDto dto)
    {
        var ur = new UserApplicationRole { Id = Guid.NewGuid(), UserId = dto.UserId, ApplicationId = dto.ApplicationId, RoleId = dto.RoleId };
        _ctx.UserApplicationRoles.Add(ur); await _ctx.SaveChangesAsync();
        return new UserApplicationRoleDto(ur.Id, ur.CreatedAt, ur.UserId, ur.ApplicationId, ur.RoleId);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var ur = await _ctx.UserApplicationRoles.FindAsync(id);
        if (ur == null) return false;
        _ctx.UserApplicationRoles.Remove(ur); await _ctx.SaveChangesAsync(); return true;
    }
}

// ==================== PERMISSION SERVICE ====================
public class PermissionService : IPermissionService
{
    private readonly AppDbContext _ctx;
    public PermissionService(AppDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<PermissionDto>> GetAllAsync() =>
        await _ctx.Permissions.Select(p => new PermissionDto(p.Id, p.Resource, p.Action, p.IsAllowed, p.CreatedAt, p.UserId)).ToListAsync();

    public async Task<PermissionDto?> GetByIdAsync(Guid id)
    {
        var p = await _ctx.Permissions.FindAsync(id);
        return p == null ? null : new PermissionDto(p.Id, p.Resource, p.Action, p.IsAllowed, p.CreatedAt, p.UserId);
    }

    public async Task<PermissionDto> CreateAsync(CreatePermissionDto dto)
    {
        var p = new Permission { Id = Guid.NewGuid(), Resource = dto.Resource, Action = dto.Action, IsAllowed = dto.IsAllowed, UserId = dto.UserId };
        _ctx.Permissions.Add(p); await _ctx.SaveChangesAsync();
        return new PermissionDto(p.Id, p.Resource, p.Action, p.IsAllowed, p.CreatedAt, p.UserId);
    }

    public async Task<PermissionDto?> UpdateAsync(Guid id, UpdatePermissionDto dto)
    {
        var p = await _ctx.Permissions.FindAsync(id);
        if (p == null) return null;
        p.Resource = dto.Resource; p.Action = dto.Action; p.IsAllowed = dto.IsAllowed;
        await _ctx.SaveChangesAsync();
        return new PermissionDto(p.Id, p.Resource, p.Action, p.IsAllowed, p.CreatedAt, p.UserId);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var p = await _ctx.Permissions.FindAsync(id);
        if (p == null) return false;
        _ctx.Permissions.Remove(p); await _ctx.SaveChangesAsync(); return true;
    }
}

// ==================== AUTH SERVICE ====================
public class AuthService : IAuthService
{
    private readonly AppDbContext _ctx;
    private readonly IConfiguration _cfg;
    public AuthService(AppDbContext ctx, IConfiguration cfg) { _ctx = ctx; _cfg = cfg; }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _ctx.Users.Include(u => u.UserApplicationRoles).ThenInclude(ur => ur.Application)
            .Include(u => u.UserApplicationRoles).ThenInclude(ur => ur.Role)
            .Include(u => u.UserApplicationRoles).ThenInclude(ur => ur.Role).ThenInclude(r => r!.RoleFunctionalities).ThenInclude(rf => rf.Functionality)
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) return null;

        user.LastLoginAt = DateTime.UtcNow;
        await _ctx.SaveChangesAsync();

        var claims = BuildClaims(user);
        var token = GenerateToken(user, claims);
        var refreshToken = GenerateRefreshToken();

        return new AuthResponse(token, refreshToken, MapUser(user), claims);
    }

    public async Task<AuthResponse?> SsoLoginAsync(SsoLoginRequest request)
    {
        var user = await _ctx.Users.Include(u => u.UserApplicationRoles).ThenInclude(ur => ur.Application)
            .Include(u => u.UserApplicationRoles).ThenInclude(ur => ur.Role)
            .Include(u => u.UserApplicationRoles).ThenInclude(ur => ur.Role).ThenInclude(r => r!.RoleFunctionalities).ThenInclude(rf => rf.Functionality)
            .FirstOrDefaultAsync(u => u.SsoProvider == request.Provider && u.SsoId == request.Token);

        if (user == null) return null;

        user.LastLoginAt = DateTime.UtcNow;
        await _ctx.SaveChangesAsync();

        var claims = BuildClaims(user);
        var token = GenerateToken(user, claims);
        var refreshToken = GenerateRefreshToken();

        return new AuthResponse(token, refreshToken, MapUser(user), claims);
    }

    public Task<AuthResponse?> RefreshTokenAsync(RefreshTokenRequest request)
    {
        return Task.FromResult<AuthResponse?>(null);
    }

    private List<UserApplicationClaim> BuildClaims(User user)
    {
        var claims = new List<UserApplicationClaim>();
        foreach (var ur in user.UserApplicationRoles)
        {
            if (ur.Application == null || ur.Role == null) continue;
            var perms = ur.Role.RoleFunctionalities.Select(rf => $"{rf.Functionality?.Code}:{GetPermissionString(rf)}").ToList();
            claims.Add(new UserApplicationClaim(ur.ApplicationId, ur.Application.Name, ur.RoleId, ur.Role.Name, perms));
        }
        return claims;
    }

    private static string GetPermissionString(RoleFunctionality rf)
    {
        var parts = new List<string>();
        if (rf.CanCreate) parts.Add("C");
        if (rf.CanRead) parts.Add("R");
        if (rf.CanUpdate) parts.Add("U");
        if (rf.CanDelete) parts.Add("D");
        return string.Join("", parts);
    }

    private string GenerateToken(User user, List<UserApplicationClaim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg["Jwt:Key"] ?? "your-super-secret-key-with-at-least-32-characters-long"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("isSuperAdmin", user.IsSuperAdmin.ToString())
        };

        foreach (var c in claims)
        {
            tokenClaims.Add(new Claim($"app_{c.ApplicationId}", System.Text.Json.JsonSerializer.Serialize(c)));
        }

        var token = new JwtSecurityToken(
            issuer: _cfg["Jwt:Issuer"] ?? "AccessControlSaaS",
            audience: _cfg["Jwt:Audience"] ?? "AccessControlSaaS-Client",
            claims: tokenClaims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken() => Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    private static UserDto MapUser(User u) => new(u.Id, u.Name, u.Email, u.Status, u.IsSuperAdmin, u.SsoProvider, u.CreatedAt, u.LastLoginAt, u.CompanyId);
}

// ==================== DASHBOARD SERVICE ====================
public class DashboardService : IDashboardService
{
    private readonly AppDbContext _ctx;
    public DashboardService(AppDbContext ctx) => _ctx = ctx;

    public async Task<DashboardSummaryDto> GetSummaryAsync()
    {
        var totalCompanies = await _ctx.Companies.CountAsync();
        var totalApps = await _ctx.Applications.CountAsync();
        var totalUsers = await _ctx.Users.CountAsync();
        var totalRoles = await _ctx.Roles.CountAsync();
        var activeUsers = await _ctx.Users.CountAsync(u => u.Status == UserStatus.Active);
        var inactiveUsers = await _ctx.Users.CountAsync(u => u.Status != UserStatus.Active);
        return new DashboardSummaryDto(totalCompanies, totalApps, totalUsers, totalRoles, activeUsers, inactiveUsers);
    }

    public async Task<IEnumerable<RecentActivityDto>> GetRecentActivityAsync(int count)
    {
        var logs = await _ctx.AuditLogs.OrderByDescending(l => l.CreatedAt).Take(count).ToListAsync();
        return logs.Select(l => new RecentActivityDto($"{l.Action} em {l.EntityType}", l.Action.ToString(), l.CreatedAt));
    }
}
