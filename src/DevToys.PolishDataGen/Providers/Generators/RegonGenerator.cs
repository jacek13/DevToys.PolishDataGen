using DevToys.PolishDataGen.Interfaces;

namespace DevToys.PolishDataGen.Providers.Generators;

public class RegonGenerator : IPolishIdGenerator
{
    public ushort[] ControlMask { get; } = { 8, 9, 2, 3, 4, 5, 6, 7 };

    private int[] RegonOddPrefixes { get; } = Enumerable.Range(1, 97).Where(x => x % 2 != 0).ToArray();

    private int[] RegonEvenPrefixes { get; } = Enumerable.Range(0, 38).Where(x => x % 2 == 0).ToArray();

    private readonly Random _random = new Random();

    public int CalculateControlNumber(string value)
    {
        var regonDigits = new int[ControlMask.Length];
        for (ushort i = 0; i < value.Length - 1; i++)
        {
            var digit = (value[i] - '0') * ControlMask[i];
            regonDigits[i] = digit;
        }

        var result = regonDigits.Sum() % 11;
        return result == 10 ? 0 : result;
    }

    public string Create()
    {
        var shufflePrefix = _random.Next(0, 100);
        var prefix = shufflePrefix % 2 == 0
            ? RegonEvenPrefixes[_random.Next(0, RegonEvenPrefixes.Length)]
            : RegonOddPrefixes[_random.Next(0, RegonOddPrefixes.Length)];
        var serialNumber = _random.Next(0, 999999);

        var regon = $"{prefix:D2}{serialNumber:D6}d";
        return $"{regon.AsSpan(0, 8)}{(char)(CalculateControlNumber(regon) + '0')}";
    }

    public IEnumerable<string> CreateMany(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return Create();
        }
    }
}
