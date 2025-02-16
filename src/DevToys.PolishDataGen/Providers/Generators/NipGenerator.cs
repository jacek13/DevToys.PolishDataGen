using DevToys.PolishDataGen.Interfaces;
using DevToys.PolishDataGen.Providers.Common;

namespace DevToys.PolishDataGen.Providers.Generators;

public class NipGenerator : IPolishIdGenerator
{
    public ushort[] ControlMask { get; } = { 6, 5, 7, 2, 3, 4, 5, 6, 7 };

    private readonly Random _random = new Random();

    public int CalculateControlNumber(string value)
    {
        var nipDigits = new int[10];
        for (ushort i = 0; i < value.Length - 1; i++)
        {
            var digit = (value[i] - '0') * ControlMask[i];
            nipDigits[i] = digit;
        }

        return nipDigits.Sum() % 11;
    }

    public string Create()
    {
        var randomPrefixIndex = _random.Next(NipPrefixes.ValidPrefixes.Length - 1);
        var taxOfficeNumber = NipPrefixes.ValidPrefixes[randomPrefixIndex];
        var serialNumber = _random.Next(0, 999999);

        var nip = $"{taxOfficeNumber}{serialNumber:D6}d";
        var controlNumber = CalculateControlNumber(nip);

        return controlNumber != 10
            ? $"{nip.AsSpan(0, 9)}{(char)(controlNumber + '0')}"
            : Create();
    }

    public IEnumerable<string> CreateMany(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return Create();
        }
    }
}
