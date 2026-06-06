using Xunit;
using FluentAssertions;
using NSubstitute;
using AccessControl.Domain.Entities;
using AccessControl.Domain.ValueObjects;

namespace AccessControl.UnitTests.Domain;

public class ValueObjectTests
{
    [Fact]
    public void Endereco_Criar_ValidData_ReturnsEndereco()
    {
        var cep = "01310-100";
        var rua = "Av. Paulista";
        var numero = "1000";
        var complemento = "Apto 101";
        var bairro = "Bela Vista";
        var cidade = "Sao Paulo";
        var estado = "SP";
        var pais = "BR";

        var endereco = Endereco.Criar(cep, rua, numero, complemento, bairro, cidade, estado, pais);

        endereco.Should().NotBeNull();
        endereco.Cep.Should().Be(cep);
        endereco.Rua.Should().Be(rua);
        endereco.Cidade.Should().Be(cidade);
        endereco.Pais.Should().Be(pais);
    }

    [Fact]
    public void Endereco_Criar_EmptyCep_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Endereco.Criar("", "Rua", "1", "", "Bairro", "Cidade", "Estado", "BR"));
    }

    [Fact]
    public void Cnpj_Criar_ValidCnpj_ReturnsCnpj()
    {
        var cnpj = "11.222.333/0001-81";
        var result = Cnpj.Criar(cnpj);
        result.Should().NotBeNull();
        result.Valor.Should().Be("11222333000181");
    }

    [Fact]
    public void Cnpj_Criar_InvalidLength_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Cnpj.Criar("123"));
    }

    [Fact]
    public void Cpf_Criar_ValidCpf_ReturnsCpf()
    {
        var cpf = "123.456.789-09";
        var result = Cpf.Criar(cpf);
        result.Should().NotBeNull();
        result.Valor.Should().Be("12345678909");
    }
}

public class EntityTests
{
    [Fact]
    public void Empresa_Creation_SetsDefaults()
    {
        var empresa = new Empresa
        {
            Nome = "Test Company",
            Cnpj = "11222333000181",
            Email = "test@example.com",
            IdiomaPadrao = "pt"
        };

        empresa.Id.Should().NotBe(Guid.Empty);
        empresa.Ativa.Should().BeTrue();
        empresa.IsDeleted.Should().BeFalse();
        empresa.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        empresa.Pais.Should().Be("BR");
    }

    [Fact]
    public void Usuario_Creation_SetsDefaults()
    {
        var usuario = new Usuario
        {
            Nome = "Test User",
            Email = "user@example.com",
            Ativo = true
        };

        usuario.Id.Should().NotBe(Guid.Empty);
        usuario.Ativo.Should().BeTrue();
        usuario.EmailConfirmado.Should().BeFalse();
        usuario.MfaHabilitado.Should().BeFalse();
        usuario.TentativasFalhasLogin.Should().Be(0);
    }

    [Fact]
    public void UsuarioAtivoSpecification_ValidUser_ReturnsTrue()
    {
        var spec = new AccessControl.Domain.Specifications.UsuarioAtivoSpecification();
        var usuario = new Usuario
        {
            Nome = "Active User",
            Email = "active@example.com",
            Ativo = true,
            IsDeleted = false
        };

        spec.IsSatisfiedBy(usuario).Should().BeTrue();
    }

    [Fact]
    public void UsuarioAtivoSpecification_BlockedUser_ReturnsFalse()
    {
        var spec = new AccessControl.Domain.Specifications.UsuarioAtivoSpecification();
        var usuario = new Usuario
        {
            Nome = "Blocked User",
            Email = "blocked@example.com",
            Ativo = true,
            IsDeleted = false,
            BloqueadoAte = DateTime.UtcNow.AddHours(1)
        };

        spec.IsSatisfiedBy(usuario).Should().BeFalse();
    }
}
