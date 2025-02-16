namespace DevToys.PolishDataGen.Providers.Generators;

public class RegonGenerator : RegonGeneratorBase
{
    private readonly Random _random = new Random();

    public override string Create()
    {
        var shufflePrefix = _random.Next(0, 100);
        var prefix = shufflePrefix % 2 == 0
            ? RegonEvenPrefixes[_random.Next(0, RegonEvenPrefixes.Length - 1)]
            : RegonOddPrefixes[_random.Next(0, RegonOddPrefixes.Length - 1)];
        var serialNumber = _random.Next(0, 999999);

        var regon = $"{prefix:D2}{serialNumber:D6}d";
        return $"{regon.AsSpan(0, 8)}{(char)(CalculateControlNumber(regon) + '0')}";
    }
}
