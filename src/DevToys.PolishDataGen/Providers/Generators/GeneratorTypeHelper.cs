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
            _ => GeneratorType.Unknown,
        };
}
