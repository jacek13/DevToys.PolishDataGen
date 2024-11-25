using DevToys.PolishDataGen.Interfaces;

namespace DevToys.PolishDataGen.Providers.Generators;

public class PeselGenerator : IPolishIdGenerator
{
    public ushort[] ControlMask { get; } = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };

    private const int MinimumYearLimit = 1800;

    private const int MaximumYearLimit = 2300;

    private readonly Random _random = new Random();

    public int CalculateControlNumber(string value)
    {
        var peselDigits = new int[11];
        for (ushort i = 0; i < value.Length - 1; i++)
        {
            var digit = (value[i] - '0') * ControlMask[i];
            peselDigits[i] = digit % 10;
        }

        var result = peselDigits.Sum() % 10;
        return result == 0 ? 0 : 10 - result;
    }

    public string Create()
    {
        var year = _random.Next(MinimumYearLimit, MaximumYearLimit);
        var month = _random.Next(1, 12);
        var day = _random.Next(1, 28);
        var ppp = _random.Next(100, 999);
        var sex = _random.Next(0, 9);
        var pesel = $"{(year % 100):D2}{(GetCenturyBase(year) + month):D2}{day:D2}{ppp}{sex}d";
        return pesel.Replace('d', (char)(CalculateControlNumber(pesel) + '0'));
    }

    public IEnumerable<string> CreateMany(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return Create();
        }
    }

    private static int GetCenturyBase(int year)
        => year switch
        {
            >= 1800 and < 1900 => 80,
            >= 1900 and < 2000 => 00,
            >= 2000 and < 2100 => 20,
            >= 2100 and < 2200 => 40,
            >= 2200 and < 2300 => 60,
            _ => throw new ArgumentOutOfRangeException(nameof(year))
        };
}
