using DevToys.PolishDataGen.Interfaces;

namespace DevToys.PolishDataGen.Providers.Generators;

public abstract class RegonGeneratorBase : IPolishIdGenerator
{
    public virtual ushort[] ControlMask { get; } = { 8, 9, 2, 3, 4, 5, 6, 7 };

    protected int[] RegonOddPrefixes { get; } = Enumerable.Range(1, 97).Where(x => x % 2 != 0).ToArray();

    protected int[] RegonEvenPrefixes { get; } = Enumerable.Range(0, 38).Where(x => x % 2 == 0).ToArray();

    public virtual int CalculateControlNumber(string value)
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

    public abstract string Create();

    public virtual IEnumerable<string> CreateMany(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return Create();
        }
    }
}
