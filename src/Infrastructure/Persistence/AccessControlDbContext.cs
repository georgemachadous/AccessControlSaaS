using Microsoft.EntityFrameworkCore;
using AccessControl.Domain.Entities;

namespace AccessControl.Infrastructure.Persistence;

public class AccessControlDbContext : DbContext
{
    private readonly Domain.Interfaces.ITenantContext? _tenantContext;

    public AccessControlDbContext(DbContextOptions<AccessControlDbContext> options) : base(options) { }

    public AccessControlDbContext(DbContextOptions<AccessControlDbContext> options, Domain.Interfaces.ITenantContext tenantContext) : base(options)
    {
        _tenantContext = tenantContext;
    }

    public DbSet<Empresa> Empresas => Set<Empresa>();
    public DbSet<Filial> Filiais => Set<Filial>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Perfil> Perfis => Set<Perfil>();
    public DbSet<Funcionalidade> Funcionalidades => Set<Funcionalidade>();
    public DbSet<PerfilFuncionalidade> PerfilFuncionalidades => Set<PerfilFuncionalidade>();
    public DbSet<Aplicacao> Aplicacoes => Set<Aplicacao>();
    public DbSet<EmpresaAplicacao> EmpresaAplicacoes => Set<EmpresaAplicacao>();
    public DbSet<UsuarioPerfilAplicacao> UsuarioPerfilAplicacoes => Set<UsuarioPerfilAplicacao>();
    public DbSet<ConfiguracaoSistema> ConfiguracoesSistema => Set<ConfiguracaoSistema>();
    public DbSet<LogAuditoria> LogsAuditoria => Set<LogAuditoria>();
    public DbSet<SessaoUsuario> SessoesUsuarios => Set<SessaoUsuario>();
    public DbSet<Notificacao> Notificacoes => Set<Notificacao>();
    public DbSet<TenantConfig> TenantConfigs => Set<TenantConfig>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply global query filter for soft delete
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(AccessControlDbContext)
                    .GetMethod(nameof(SetSoftDeleteFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                    ?.MakeGenericMethod(entityType.ClrType);
                method?.Invoke(null, new object[] { modelBuilder });
            }
        }

        // Apply global query filter for multi-tenancy (except Empresa and Aplicacao)
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType)
                && entityType.ClrType != typeof(Empresa)
                && entityType.ClrType != typeof(Aplicacao)
                && entityType.ClrType != typeof(ConfiguracaoSistema))
            {
                var method = typeof(AccessControlDbContext)
                    .GetMethod(nameof(SetTenantFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                    ?.MakeGenericMethod(entityType.ClrType);
                method?.Invoke(null, new object[] { modelBuilder });
            }
        }

        // Empresa
        modelBuilder.Entity<Empresa>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Cnpj).IsUnique();
            e.HasIndex(x => x.Email);
            e.HasIndex(x => new { x.TenantId, x.Ativa });
            e.HasIndex(x => x.Nome).HasDatabaseName("IX_Empresas_Nome");
            e.Property(x => x.ConfiguracoesJSON).HasDefaultValue("{}");
            e.Property(x => x.Pais).HasDefaultValue("BR");
            e.HasMany(x => x.Filiais).WithOne(x => x.Empresa).HasForeignKey(x => x.EmpresaId).OnDelete(DeleteBehavior.Cascade);
            e.HasMany(x => x.Usuarios).WithOne().HasForeignKey("TenantId").OnDelete(DeleteBehavior.Restrict);
            e.HasMany(x => x.EmpresaAplicacoes).WithOne(x => x.Empresa).HasForeignKey(x => x.EmpresaId).OnDelete(DeleteBehavior.Cascade);
            e.HasMany(x => x.Perfis).WithOne(x => x.Empresa).HasForeignKey(x => x.EmpresaId).OnDelete(DeleteBehavior.Cascade);
        });

        // Filial
        modelBuilder.Entity<Filial>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.EmpresaId, x.Codigo }).IsUnique();
            e.HasIndex(x => x.TenantId);
            e.HasMany(x => x.Usuarios).WithOne(x => x.Filial).HasForeignKey(x => x.FilialId).OnDelete(DeleteBehavior.SetNull);
        });

        // Usuario
        modelBuilder.Entity<Usuario>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Email);
            e.HasIndex(x => new { x.TenantId, x.Ativo });
            e.HasIndex(x => x.Cpf).IsUnique().HasFilter("[Cpf] IS NOT NULL");
            e.HasMany(x => x.UsuarioPerfilAplicacoes).WithOne(x => x.Usuario).HasForeignKey(x => x.UsuarioId).OnDelete(DeleteBehavior.Cascade);
            e.HasMany(x => x.Sessoes).WithOne(x => x.Usuario).HasForeignKey(x => x.UsuarioId).OnDelete(DeleteBehavior.Cascade);
            e.HasMany(x => x.Notificacoes).WithOne(x => x.Usuario).HasForeignKey(x => x.UsuarioId).OnDelete(DeleteBehavior.Cascade);
        });

        // Perfil
        modelBuilder.Entity<Perfil>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.EmpresaId, x.Nome }).IsUnique();
            e.HasMany(x => x.PerfilFuncionalidades).WithOne(x => x.Perfil).HasForeignKey(x => x.PerfilId).OnDelete(DeleteBehavior.Cascade);
            e.HasMany(x => x.UsuarioPerfilAplicacoes).WithOne(x => x.Perfil).HasForeignKey(x => x.PerfilId).OnDelete(DeleteBehavior.Cascade);
        });

        // Funcionalidade
        modelBuilder.Entity<Funcionalidade>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Codigo).IsUnique();
            e.HasIndex(x => new { x.AplicacaoId, x.Ativa });
            e.HasMany(x => x.PerfilFuncionalidades).WithOne(x => x.Funcionalidade).HasForeignKey(x => x.FuncionalidadeId).OnDelete(DeleteBehavior.Cascade);
        });

        // PerfilFuncionalidade
        modelBuilder.Entity<PerfilFuncionalidade>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.PerfilId, x.FuncionalidadeId }).IsUnique();
        });

        // Aplicacao
        modelBuilder.Entity<Aplicacao>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Codigo).IsUnique();
            e.HasMany(x => x.EmpresaAplicacoes).WithOne(x => x.Aplicacao).HasForeignKey(x => x.AplicacaoId).OnDelete(DeleteBehavior.Cascade);
            e.HasMany(x => x.Funcionalidades).WithOne(x => x.Aplicacao).HasForeignKey(x => x.AplicacaoId).OnDelete(DeleteBehavior.SetNull);
        });

        // EmpresaAplicacao
        modelBuilder.Entity<EmpresaAplicacao>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.EmpresaId, x.AplicacaoId }).IsUnique();
        });

        // UsuarioPerfilAplicacao
        modelBuilder.Entity<UsuarioPerfilAplicacao>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.UsuarioId, x.AplicacaoId });
            e.HasIndex(x => new { x.UsuarioId, x.PerfilId, x.AplicacaoId }).IsUnique();
        });

        // ConfiguracaoSistema
        modelBuilder.Entity<ConfiguracaoSistema>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Chave).IsUnique();
        });

        // LogAuditoria
        modelBuilder.Entity<LogAuditoria>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.TenantId, x.Entidade, x.EntidadeId });
            e.HasIndex(x => x.CreatedAt);
            e.HasIndex(x => x.CorrelationId);
            e.HasIndex(x => x.Acao);
        });

        // SessaoUsuario
        modelBuilder.Entity<SessaoUsuario>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.TokenJti).IsUnique();
            e.HasIndex(x => x.RefreshToken).IsUnique();
            e.HasIndex(x => new { x.UsuarioId, x.IsValida });
            e.HasIndex(x => x.DataExpiracao);
        });

        // Notificacao
        modelBuilder.Entity<Notificacao>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.UsuarioId, x.Lida });
            e.HasIndex(x => x.CreatedAt);
        });

        // TenantConfig
        modelBuilder.Entity<TenantConfig>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.EmpresaId, x.Chave }).IsUnique();
        });
    }

    private static void SetSoftDeleteFilter<TEntity>(ModelBuilder builder) where TEntity : BaseEntity
    {
        builder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
    }

    private static void SetTenantFilter<TEntity>(ModelBuilder builder) where TEntity : BaseEntity
    {
        builder.Entity<TEntity>().HasQueryFilter(e => e.TenantId == Guid.Empty || e.TenantId == Guid.Empty); // Will be overridden at runtime
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = _tenantContext?.CurrentUserId ?? "system";
                    if (_tenantContext is not null && entry.Entity.TenantId == Guid.Empty)
                        entry.Entity.TenantId = _tenantContext.CurrentTenantId;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = _tenantContext?.CurrentUserId ?? "system";
                    break;
                case EntityState.Deleted:
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    entry.Entity.IsDeleted = true;
                    entry.State = EntityState.Modified;
                    break;
            }
        }
        return await base.SaveChangesAsync(cancellationToken);
    }
}
