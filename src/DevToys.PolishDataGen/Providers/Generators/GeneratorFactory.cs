using DevToys.PolishDataGen.Interfaces;
using DevToys.PolishDataGen.Providers.Common;

namespace DevToys.PolishDataGen.Providers.Generators;

internal static class GeneratorFactory
{
    public static IPolishIdGenerator Create(IdType type)
        => type switch
        {
            IdType.Pesel => new PeselGenerator(),
            IdType.Regon => new RegonGenerator(),
            IdType.RegonLong => new RegonLongGenerator(),
            IdType.Nip => new NipGenerator(),
            IdType.PolishIdentityCard => new IdentityCardNumberGenerator(),
            _ => throw new NotImplementedException($"Not supported type = {type.ToString()}!"),
        };
}
