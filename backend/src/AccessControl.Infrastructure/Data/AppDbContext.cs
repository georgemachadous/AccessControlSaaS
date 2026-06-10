using AccessControl.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccessControl.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<ApplicationEntity> Applications => Set<ApplicationEntity>();
    public DbSet<Functionality> Functionalities => Set<Functionality>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<RoleFunctionality> RoleFunctionalities => Set<RoleFunctionality>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserApplicationRole> UserApplicationRoles => Set<UserApplicationRole>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Company>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Name);
            e.HasMany(x => x.Branches).WithOne(x => x.Company).HasForeignKey(x => x.CompanyId);
            e.HasMany(x => x.Applications).WithOne(x => x.Company).HasForeignKey(x => x.CompanyId);
            e.HasMany(x => x.Users).WithOne(x => x.Company).HasForeignKey(x => x.CompanyId);
        });

        modelBuilder.Entity<Branch>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Name);
        });

        modelBuilder.Entity<ApplicationEntity>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Name);
            e.HasMany(x => x.Functionalities).WithOne(x => x.Application).HasForeignKey(x => x.ApplicationId);
            e.HasMany(x => x.Roles).WithOne(x => x.Application).HasForeignKey(x => x.ApplicationId);
            e.HasMany(x => x.UserApplicationRoles).WithOne(x => x.Application).HasForeignKey(x => x.ApplicationId);
        });

        modelBuilder.Entity<Functionality>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Code);
        });

        modelBuilder.Entity<Role>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Name);
            e.HasMany(x => x.RoleFunctionalities).WithOne(x => x.Role).HasForeignKey(x => x.RoleId);
            e.HasMany(x => x.UserApplicationRoles).WithOne(x => x.Role).HasForeignKey(x => x.RoleId);
        });

        modelBuilder.Entity<RoleFunctionality>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.RoleId, x.FunctionalityId }).IsUnique();
        });

        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Email).IsUnique();
            e.HasMany(x => x.UserApplicationRoles).WithOne(x => x.User).HasForeignKey(x => x.UserId);
            e.HasMany(x => x.Permissions).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        });

        modelBuilder.Entity<UserApplicationRole>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.UserId, x.ApplicationId, x.RoleId }).IsUnique();
        });

        modelBuilder.Entity<Permission>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.UserId, x.Resource, x.Action }).IsUnique();
        });

        modelBuilder.Entity<AuditLog>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.CreatedAt);
        });
    }
}
