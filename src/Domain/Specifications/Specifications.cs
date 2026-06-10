namespace AccessControl.Domain.Specifications;

public interface ISpecification<T>
{
    bool IsSatisfiedBy(T entity);
}

public class UsuarioAtivoSpecification : ISpecification<Entities.Usuario>
{
    public bool IsSatisfiedBy(Entities.Usuario usuario) =>
        usuario.Ativo && !usuario.IsDeleted && (usuario.BloqueadoAte is null || usuario.BloqueadoAte < DateTime.UtcNow);
}

public class EmpresaAtivaSpecification : ISpecification<Entities.Empresa>
{
    public bool IsSatisfiedBy(Entities.Empresa empresa) =>
        empresa.Ativa && !empresa.IsDeleted;
}

public class AplicacaoAtivaSpecification : ISpecification<Entities.Aplicacao>
{
    public bool IsSatisfiedBy(Entities.Aplicacao app) =>
        app.Ativa && !app.IsDeleted;
}

public class SessaoValidaSpecification : ISpecification<Entities.SessaoUsuario>
{
    public bool IsSatisfiedBy(Entities.SessaoUsuario sessao) =>
        sessao.IsValida && sessao.DataExpiracao > DateTime.UtcNow;
}
