using DevToys.PolishDataGen.Interfaces;
using DevToys.PolishDataGen.Providers.Generators;

namespace DevToys.PolishDataGen.Cli;

internal static class GeneratorFactory
{
    public static IPolishIdGenerator Create(GeneratorType type)
        => type switch
        {
            GeneratorType.Pesel => new PeselGenerator(),
            _ => throw new NotImplementedException($"Not supported type = {type.ToString()}!"),
        };
}
