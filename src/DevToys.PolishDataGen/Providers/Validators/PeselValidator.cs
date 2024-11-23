using DevToys.PolishDataGen.Interfaces;

namespace DevToys.PolishDataGen.Providers.Validators;

public class PeselValidator : IPolishIdValidator
{
    public ushort[] ControlMask { get; } = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };

    private const ushort MmStartIndex = 2;

    private const ushort DdStartIndex = 4;

    private const ushort KStartIndex = 10;

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

    public bool IsValid(string value)
    {
        // TODO refactor .Substring() method call to reduce memory allocation
        if (string.IsNullOrWhiteSpace(value)) return false;
        if (value.Length != 11) return false;
        if (!value.All(char.IsAsciiDigit)) return false;
        if (value.All(c => c == '0') || value.All(c => c == '9')) return false;
        if (!((int.Parse(value.Substring(MmStartIndex, 2)) % 20) is > 0 and < 13)) return false;
        if (!(int.Parse(value.Substring(DdStartIndex, 2)) is > 0 and <= 31)) return false;
        if (CalculateControlNumber(value) != (value[KStartIndex] - '0')) return false;

        return true;
    }

    public (bool Result, IEnumerable<string> Messages) IsValidExplained(string value)
    {
        var validationMessages = GetValidationMessages(value);

        return validationMessages.Any()
            ? (false, validationMessages)
            : (true, []);
    }

    private IEnumerable<string> GetValidationMessages(string value)
    {
        // TODO refactor .Substring() method call to reduce memory allocation
        // TODO prepare some strings in PolishDataGen.resx or move to some const dict structure
        if (string.IsNullOrWhiteSpace(value)) yield return "PESEL must contain some values";
        if (value.Length != 11) yield return "PESEL must be 11 characters long";
        if (!value.All(char.IsAsciiDigit)) yield return "PESEL must contain only digits";
        if (value.All(c => c == '0') || value.All(c => c == '9')) yield return "PESEL cannot be composed only from 0 or 9 digits";
        if (!((int.Parse(value.Substring(MmStartIndex, 2)) % 20) is > 0 and < 13)) yield return "PESEL Month part must be in range 1 to 12";
        if (!(int.Parse(value.Substring(DdStartIndex, 2)) is > 0 and <= 31)) yield return "PESEL Day part must be in range 1 to 31";
        if (value.Length == 11 && CalculateControlNumber(value) != (value[KStartIndex] - '0')) yield return "PESEL invalid control number";
    }
}
