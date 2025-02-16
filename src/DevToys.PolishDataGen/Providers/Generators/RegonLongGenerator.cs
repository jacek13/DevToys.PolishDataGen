namespace DevToys.PolishDataGen.Providers.Generators;

public class RegonLongGenerator : RegonGeneratorBase
{
    public override ushort[] ControlMask { get; } = { 2, 4, 8, 5, 0, 9, 7, 3, 6, 1, 2, 4, 8 };

    private readonly Random _random = new Random();

    public override string Create()
    {
        var shufflePrefix = _random.Next(0, 100);
        var prefix = shufflePrefix % 2 == 0
            ? RegonEvenPrefixes[_random.Next(0, RegonEvenPrefixes.Length - 1)]
            : RegonOddPrefixes[_random.Next(0, RegonOddPrefixes.Length - 1)];
        var serialNumber = _random.NextInt64(0, 99999999999);

        var regon = $"{prefix:D2}{serialNumber:D11}d";
        return $"{regon.AsSpan(0, 13)}{(char)(CalculateControlNumber(regon) + '0')}";
    }
}
