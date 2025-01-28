using DevToys.PolishDataGen.Interfaces;

namespace DevToys.PolishDataGen.Providers.Generators;

/*
 * // 9-cyfrowy
// 2 dwie cyfry
// Dla starych numerów REGON 7 cyfrowych -> 00
// Dla 1980-1990 -> nieparzyste od 01 do 97
// obecnie -> parzyste od 02 do 34 - obecnie do 38

// 14 - cyfrowy
2 4 8 5 0 9 7 3 6 1 2 4 8
 * */
public class RegonGenerator : IPolishIdGenerator
{
    public ushort[] ControlMask { get; } = { 8, 9, 2, 3, 4, 5, 6, 7 };

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
        throw new NotImplementedException();
    }

    public IEnumerable<string> CreateMany(int count)
    {
        throw new NotImplementedException();
    }
}
