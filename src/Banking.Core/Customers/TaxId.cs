using System.Text.RegularExpressions;

namespace Banking.Core.Customers;

public sealed record TaxId
{
    private static readonly int[] Multiplicador1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
    private static readonly int[] Multiplicador2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];
    private readonly string _value;

    private TaxId()
    {
        _value = default!;
    }
    
    private TaxId(string value)
    {
        _value = value;
    }

    public static TaxId From(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (!IsValid(value))
            throw new ArgumentException($"Invalid TaxID: {value} .", nameof(value));
        var input = Normalize(value);
        var output = new TaxId(input);
        return output;
    }

    public string ToString(string format)
    {
        var numeric = Regex.Replace(_value, @"[^\d]", string.Empty);

        return format switch
               {
                   "F" => Convert.ToUInt64(numeric).ToString(@"000\.000\.000\-00"),
                   "N" => numeric,
                   _ => throw new ArgumentException($"Formato {format} inválido",nameof(format))
               };
    }

    public override string ToString() => ToString("N");

    public static implicit operator string(TaxId value) => value.ToString();

    public static implicit operator TaxId(string value) => new(value);

    private static bool IsValid(string value)
    {
        value = Normalize(value);
        
        if (string.IsNullOrWhiteSpace(value) || value.Length != 11)
            return false;

        for (var j = 0; j < 10; j++)
            if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == value)
                return false;

        var tempCpf = value.Substring(0, 9);
        var soma = 0;

        for (var i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * Multiplicador1[i];

        var resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;

        var digito = resto.ToString();
        tempCpf += digito;
        soma = 0;
        for (var i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * Multiplicador2[i];

        resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;

        digito += resto.ToString();

        return value.EndsWith(digito);
    }

    private static string Normalize(string numero) => Regex.Replace(numero, @"[^\d]", string.Empty);

    // private sealed class JsonConverter : JsonConverter<TaxId>
    // {
    //     public override TaxId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    //     {
    //         return From(reader.GetString() ?? throw new InvalidOperationException());
    //     }
    //
    //     public override void Write(Utf8JsonWriter writer, TaxId value, JsonSerializerOptions options)
    //     {
    //         writer.WriteStringValue(value.ToString());
    //     }
    // }
}