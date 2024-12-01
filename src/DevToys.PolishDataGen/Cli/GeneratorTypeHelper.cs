namespace DevToys.PolishDataGen.Cli;

internal static class GeneratorTypeHelper
{
    public static GeneratorType ConvertToGeneratorType(string type)
        => string.IsNullOrWhiteSpace(type)
        ? GeneratorType.Unknown
        : type.Trim().ToLower() switch
        {
            "pesel" => GeneratorType.Pesel,
            _ => GeneratorType.Unknown,
        };
}
