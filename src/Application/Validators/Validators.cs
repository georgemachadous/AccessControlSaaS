using FluentValidation;
using AccessControl.Application.DTOs;

namespace AccessControl.Application.Validators;

public class CriarEmpresaValidator : AbstractValidator<CriarEmpresaDto>
{
    public CriarEmpresaValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(200);
        RuleFor(x => x.NomeFantasia).MaximumLength(200);
        RuleFor(x => x.Cnpj).NotEmpty().Length(14, 18).Matches(@"^\d{2}\.?\d{3}\.?\d{3}\/?\d{4}-?\d{2}$");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(x => x.Telefone).MaximumLength(20);
        RuleFor(x => x.IdiomaPadrao).NotEmpty().Must(x => x is "pt" or "en" or "es").WithMessage("Idioma deve ser pt, en ou es");
        RuleFor(x => x.Cep).NotEmpty().MaximumLength(15);
        RuleFor(x => x.Rua).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Cidade).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Estado).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Pais).NotEmpty().MaximumLength(2);
    }
}

public class CriarUsuarioValidator : AbstractValidator<CriarUsuarioDto>
{
    public CriarUsuarioValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(x => x.Telefone).MaximumLength(20);
        RuleFor(x => x.Cpf).MaximumLength(14).Matches(@"^\d{3}\.?\d{3}\.?\d{3}-?\d{2}$").When(x => !string.IsNullOrEmpty(x.Cpf));
        RuleFor(x => x.Idioma).Must(x => string.IsNullOrEmpty(x) || x is "pt" or "en" or "es");
        RuleFor(x => x.Senha).NotEmpty().MinimumLength(12).MaximumLength(100)
            .Matches(@"[A-Z]").WithMessage("Senha deve conter pelo menos uma letra maiúscula")
            .Matches(@"[a-z]").WithMessage("Senha deve conter pelo menos uma letra minúscula")
            .Matches(@"[0-9]").WithMessage("Senha deve conter pelo menos um número")
            .Matches(@"[^a-zA-Z0-9]").WithMessage("Senha deve conter pelo menos um caractere especial")
            .When(x => !x.IsSsoUser);
    }
}

public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Senha).NotEmpty();
    }
}

public class CriarPerfilValidator : AbstractValidator<CriarPerfilDto>
{
    public CriarPerfilValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Descricao).MaximumLength(500);
    }
}

public class CriarAplicacaoValidator : AbstractValidator<CriarAplicacaoDto>
{
    public CriarAplicacaoValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Descricao).MaximumLength(500);
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(50).Matches(@"^[A-Z_]+$");
        RuleFor(x => x.Url).NotEmpty().MaximumLength(500).Must(x => Uri.IsWellFormedUriString(x, UriKind.Absolute));
    }
}

public class AlterarSenhaValidator : AbstractValidator<AlterarSenhaDto>
{
    public AlterarSenhaValidator()
    {
        RuleFor(x => x.SenhaAtual).NotEmpty();
        RuleFor(x => x.NovaSenha).NotEmpty().MinimumLength(12).MaximumLength(100)
            .Matches(@"[A-Z]").Matches(@"[a-z]").Matches(@"[0-9]").Matches(@"[^a-zA-Z0-9]");
        RuleFor(x => x.ConfirmarNovaSenha).Equal(x => x.NovaSenha).WithMessage("As senhas não conferem");
    }
}
