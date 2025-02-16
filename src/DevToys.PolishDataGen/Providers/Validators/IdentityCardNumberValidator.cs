using DevToys.PolishDataGen.Interfaces;

namespace DevToys.PolishDataGen.Providers.Validators;

public class IdentityCardNumberValidator : IPolishIdValidator
{
    public ushort[] ControlMask => throw new NotImplementedException();

    public int CalculateControlNumber(string value)
    {
        throw new NotImplementedException();
    }

    public bool IsValid(string value)
    {
        throw new NotImplementedException();
    }

    public (bool Result, IEnumerable<string> Messages) IsValidExplained(string value)
    {
        throw new NotImplementedException();
    }
}
