using AccessControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.Infrastructure.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Users.Any(u => u.IsSuperAdmin)) return;

        var companyId = Guid.NewGuid();
        var company = new Company
        {
            Id = companyId,
            Name = "SaaS Admin",
            Document = "00.000.000/0000-00",
            Email = "admin@saas.com",
            IsActive = true
        };
        context.Companies.Add(company);

        var appId = Guid.NewGuid();
        var app = new ApplicationEntity
        {
            Id = appId,
            Name = "Access Control Admin",
            Description = "Sistema de controle de acesso",
            CompanyId = companyId,
            IsActive = true
        };
        context.Applications.Add(app);

        var funcIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        var funcs = new List<Functionality>
        {
            new() { Id = funcIds[0], Name = "Empresas", Code = "COMPANY", ApplicationId = appId },
            new() { Id = funcIds[1], Name = "Filiais", Code = "BRANCH", ApplicationId = appId },
            new() { Id = funcIds[2], Name = "Aplicacoes", Code = "APP", ApplicationId = appId },
            new() { Id = funcIds[3], Name = "Funcionalidades", Code = "FUNC", ApplicationId = appId },
            new() { Id = funcIds[4], Name = "Usuarios", Code = "USER", ApplicationId = appId }
        };
        context.Functionalities.AddRange(funcs);

        var roleId = Guid.NewGuid();
        var role = new Role
        {
            Id = roleId,
            Name = "Super Admin",
            Description = "Acesso total ao sistema",
            ApplicationId = appId,
            IsActive = true
        };
        context.Roles.Add(role);

        var roleFuncs = funcIds.Select(fid => new RoleFunctionality
        {
            Id = Guid.NewGuid(),
            RoleId = roleId,
            FunctionalityId = fid,
            CanCreate = true, CanRead = true, CanUpdate = true, CanDelete = true
        }).ToList();
        context.RoleFunctionalities.AddRange(roleFuncs);

        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Super Admin",
            Email = "superadmin@admin.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("SuperAdmin@123"),
            Status = UserStatus.Active,
            IsSuperAdmin = true,
            CompanyId = companyId
        };
        context.Users.Add(user);

        context.UserApplicationRoles.Add(new UserApplicationRole
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ApplicationId = appId,
            RoleId = roleId
        });

        context.SaveChanges();
    }
}
