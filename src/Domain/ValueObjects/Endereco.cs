namespace AccessControl.Domain.ValueObjects;

public record Endereco
{
    public string Cep { get; init; } = string.Empty;
    public string Rua { get; init; } = string.Empty;
    public string Numero { get; init; } = string.Empty;
    public string Complemento { get; init; } = string.Empty;
    public string Bairro { get; init; } = string.Empty;
    public string Cidade { get; init; } = string.Empty;
    public string Estado { get; init; } = string.Empty;
    public string Pais { get; init; } = "BR";

    public static Endereco Criar(string cep, string rua, string numero, string complemento, string bairro, string cidade, string estado, string pais)
    {
        if (string.IsNullOrWhiteSpace(cep)) throw new ArgumentException("CEP obrigatorio");
        if (string.IsNullOrWhiteSpace(rua)) throw new ArgumentException("Rua obrigatoria");
        if (string.IsNullOrWhiteSpace(cidade)) throw new ArgumentException("Cidade obrigatoria");
        return new Endereco { Cep = cep, Rua = rua, Numero = numero, Complemento = complemento, Bairro = bairro, Cidade = cidade, Estado = estado, Pais = pais };
    }
}

public record Cnpj
{
    public string Valor { get; init; } = string.Empty;

    public static Cnpj Criar(string cnpj)
    {
        var digits = new string(cnpj.Where(char.IsDigit).ToArray());
        if (digits.Length != 14) throw new ArgumentException("CNPJ deve ter 14 digitos");
        return new Cnpj { Valor = digits };
    }

    public string Formatado => $"{Valor.Substring(0, 2)}.{Valor.Substring(2, 3)}.{Valor.Substring(5, 3)}/{Valor.Substring(8, 4)}-{Valor.Substring(12)}";
}

public record Cpf
{
    public string Valor { get; init; } = string.Empty;

    public static Cpf Criar(string cpf)
    {
        var digits = new string(cpf.Where(char.IsDigit).ToArray());
        if (digits.Length != 11) throw new ArgumentException("CPF deve ter 11 digitos");
        return new Cpf { Valor = digits };
    }

    public string Formatado => $"{Valor.Substring(0, 3)}.{Valor.Substring(3, 3)}.{Valor.Substring(6, 3)}-{Valor.Substring(9)}";
}
