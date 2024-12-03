using DevToys.PolishDataGen.Interfaces;

namespace DevToys.PolishDataGen.Providers.Generators;

internal static class GeneratorFactory
{
    public static IPolishIdGenerator Create(GeneratorType type)
        => type switch
        {
            GeneratorType.Pesel => new PeselGenerator(),
            _ => throw new NotImplementedException($"Not supported type = {type.ToString()}!"),
        };
}
