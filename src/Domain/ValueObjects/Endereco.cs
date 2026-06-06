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
        if (string.IsNullOrWhiteSpace(cep)) throw new ArgumentException("CEP obrigatório");
        if (string.IsNullOrWhiteSpace(rua)) throw new ArgumentException("Rua obrigatória");
        if (string.IsNullOrWhiteSpace(cidade)) throw new ArgumentException("Cidade obrigatória");
        return new Endereco { Cep = cep, Rua = rua, Numero = numero, Complemento = complemento, Bairro = bairro, Cidade = cidade, Estado = estado, Pais = pais };
    }
}

public record Cnpj
{
    public string Valor { get; init; } = string.Empty;

    public static Cnpj Criar(string cnpj)
    {
        var digits = new string(cnpj.Where(char.IsDigit).ToArray());
        if (digits.Length != 14) throw new ArgumentException("CNPJ deve ter 14 dígitos");
        return new Cnpj { Valor = digits };
    }

    public string Formatado => $"{Valor[..2]}.{Valor[2:5]}.{Valor[5:8]}/{Valor[8:12]}-{Valor[12..]}";
}

public record Cpf
{
    public string Valor { get; init; } = string.Empty;

    public static Cpf Criar(string cpf)
    {
        var digits = new string(cpf.Where(char.IsDigit).ToArray());
        if (digits.Length != 11) throw new ArgumentException("CPF deve ter 11 dígitos");
        return new Cpf { Valor = digits };
    }

    public string Formatado => $"{Valor[..3]}.{Valor[3:6]}.{Valor[6:9]}-{Valor[9..]}";
}
