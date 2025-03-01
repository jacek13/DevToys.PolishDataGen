using DevToys.PolishDataGen.Interfaces;
using DevToys.PolishDataGen.Providers.Common;

namespace DevToys.PolishDataGen.Providers.Validators;

internal static class ValidatorFactory
{
    public static IPolishIdValidator Create(IdType type)
        => type switch
        {
            IdType.Pesel => new PeselValidator(),
            IdType.Regon => new RegonValidator(),
            IdType.RegonLong => new RegonValidator(),
            IdType.Nip => new NipValidator(),
            IdType.PolishIdentityCard => new IdentityCardNumberValidator(),
            _ => throw new NotImplementedException($"Not supported type = {type.ToString()}!"),
        };
}
