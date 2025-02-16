using DevToys.PolishDataGen.Interfaces;

namespace DevToys.PolishDataGen.Providers.Generators;

public class NipGenerator : IPolishIdGenerator
{
    public ushort[] ControlMask => throw new NotImplementedException();

    public int CalculateControlNumber(string value)
    {
        throw new NotImplementedException();
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
