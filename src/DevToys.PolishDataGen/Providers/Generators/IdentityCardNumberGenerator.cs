using DevToys.PolishDataGen.Interfaces;

namespace DevToys.PolishDataGen.Providers.Generators;

public class IdentityCardNumberGenerator : IPolishIdGenerator
{
    public ushort[] ControlMask { get; } = { 7, 3, 1, 7, 3, 1, 7, 3 };

    private const ushort ControlNumberIndex = 3;

    private const ushort InitialAlphabetValue = 10;

    private readonly Random _random = new Random();

    public IEnumerable<string> CreateMany(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return Create();
        }
    }

    public string Create()
    {
        var prefix = GetPrefix();
        var serialNumber = _random.Next(0, 99999);
        var controlNumber = CalculateControlNumber($"{prefix}{serialNumber:D5}");

        return $"{prefix}{controlNumber:D1}{serialNumber:D5}";
    }

    private string GetPrefix()
        => $"{GetCharFromRange('A', 'D')}{GetCharFromRange()}{GetCharFromRange()}";

    private char GetCharFromRange(char minValue = 'A', char maxValue = 'Z')
    {
        var character = (char)_random.Next(minValue, maxValue);
        return character is 'O' or 'Q' ? ++character : character;
    }

    public int CalculateControlNumber(string value)
    {
        var values = new int[8];
        for (ushort i = 0; i < value.Length; i++)
        {
            var asciiOffset = i < ControlNumberIndex ? 'A' - InitialAlphabetValue : '0';
            var number = (value[i] - asciiOffset) * ControlMask[i];
            values[i] = number;
        }

        return values.Sum() % 10;
    }
}
