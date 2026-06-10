namespace AccessControl.Domain.Entities;

public class PerfilFuncionalidade : BaseEntity
{
    public Guid PerfilId { get; set; }
    public Guid FuncionalidadeId { get; set; }
    public bool PermiteCriar { get; set; } = true;
    public bool PermiteLer { get; set; } = true;
    public bool PermiteAtualizar { get; set; } = true;
    public bool PermiteDeletar { get; set; } = true;
    public bool PermiteExecutar { get; set; } = true;
    public Perfil Perfil { get; set; } = null!;
    public Funcionalidade Funcionalidade { get; set; } = null!;
}
