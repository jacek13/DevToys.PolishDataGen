namespace DevToys.PolishDataGen.Providers.Common;

internal static class IdTypeHelper
{
    public static IdType ConvertToIdType(string type)
        => string.IsNullOrWhiteSpace(type)
        ? IdType.Unknown
        : type.Trim().ToLower() switch
        {
            "pesel" => IdType.Pesel,
            "regon" => IdType.Regon,
            "regon-long" => IdType.RegonLong,
            "nip" => IdType.Nip,
            "identity-card-number" => IdType.PolishIdentityCard,
            _ => IdType.Unknown,
        };
}
