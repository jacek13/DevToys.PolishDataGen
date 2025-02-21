namespace DevToys.PolishDataGen.Providers.Generators;

internal static class GeneratorTypeHelper
{
    public static GeneratorType ConvertToGeneratorType(string type)
        => string.IsNullOrWhiteSpace(type)
        ? GeneratorType.Unknown
        : type.Trim().ToLower() switch
        {
            "pesel" => GeneratorType.Pesel,
            "regon" => GeneratorType.Regon,
            "regon-long" => GeneratorType.RegonLong,
            "nip" => GeneratorType.Nip,
            "identity-card-number" => GeneratorType.PolishIdentityCard,
            _ => GeneratorType.Unknown,
        };
}
