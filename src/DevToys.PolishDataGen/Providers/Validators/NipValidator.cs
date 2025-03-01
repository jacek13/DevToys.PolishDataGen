using DevToys.PolishDataGen.Interfaces;
using DevToys.PolishDataGen.Providers.Common;

namespace DevToys.PolishDataGen.Providers.Validators;

public class NipValidator : IPolishIdValidator
{
    public ushort[] ControlMask { get; } = { 6, 5, 7, 2, 3, 4, 5, 6, 7 };

    private const ushort KStartIndex = 9;

    private const ushort PrefixStartIndex = 0;

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

    public bool IsValid(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;
        if (value.Length != 10)
            return false;
        if (!value.All(char.IsAsciiDigit))
            return false;
        if (value.All(c => c == '0') || value.All(c => c == '9'))
            return false;
        if (!NipPrefixes.ValidPrefixes.Any(p => p.AsSpan().CompareTo(value.AsSpan(PrefixStartIndex, 3), StringComparison.Ordinal) == 0))
            return false;
        if (CalculateControlNumber(value) != (value[KStartIndex] - '0'))
            return false;

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
        if (string.IsNullOrWhiteSpace(value))
        {
            yield return Strings.PolishDataGen.NipValidationNotEmptyMessage;
            yield break;
        }
        if (value.Length != 10)
        {
            yield return Strings.PolishDataGen.NipValidationLengthMessage;
            yield break;
        }
        if (!value.All(char.IsAsciiDigit))
        {
            yield return Strings.PolishDataGen.NipValidationOnlyDigitsMessage;
            yield break;
        }
        if (value.All(c => c == '0') || value.All(c => c == '9'))
            yield return Strings.PolishDataGen.NipValidationExtremeValuesMessage;
        if (!NipPrefixes.ValidPrefixes.Any(p => p.AsSpan().CompareTo(value.AsSpan(PrefixStartIndex, 3), StringComparison.Ordinal) == 0))
            yield return Strings.PolishDataGen.NipValidationInvalidPrefix;
        if (CalculateControlNumber(value) != (value[KStartIndex] - '0'))
            yield return Strings.PolishDataGen.NipValidationControlNumberMessage;
    }
}
