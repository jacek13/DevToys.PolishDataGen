using DevToys.PolishDataGen.Interfaces;

namespace DevToys.PolishDataGen.Providers.Validators;

public class RegonValidator : IPolishIdValidator
{
    public ushort[] ControlMask { get; } = { 8, 9, 2, 3, 4, 5, 6, 7 };

    public ushort[] ControlMaskLong { get; } = { 2, 4, 8, 5, 0, 9, 7, 3, 6, 1, 2, 4, 8 };

    private const ushort KStartIndex = 8;

    private const ushort KStartIndexLong = 13;

    public int CalculateControlNumber(string value)
    {
        var regonDigits = new int[value.Length == 9 ? 8 : 13];
        for (ushort i = 0; i < value.Length - 1; i++)
        {
            var digit = (value[i] - '0') * (value.Length == 9 ? ControlMask[i] : ControlMaskLong[i]);
            regonDigits[i] = digit;
        }

        var result = regonDigits.Sum() % 11;
        return result == 10 ? 0 : result;
    }

    public bool IsValid(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;
        if (value.Length != 9 && value.Length != 14)
            return false;
        if (!value.All(char.IsAsciiDigit))
            return false;
        if (value.All(c => c == '0') || value.All(c => c == '9'))
            return false;
        if (CalculateControlNumber(value) != (value[value.Length == 9 ? KStartIndex : KStartIndexLong] - '0'))
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
            yield return Strings.PolishDataGen.RegonValidationNotEmptyMessage;
            yield break;
        }
        if (value.Length != 9 && value.Length != 14)
        {
            yield return Strings.PolishDataGen.RegonValidationLengthMessage;
            yield break;
        }
        if (!value.All(char.IsAsciiDigit))
            yield return Strings.PolishDataGen.RegonValidationOnlyDigitsMessage;
        if (value.All(c => c == '0') || value.All(c => c == '9'))
            yield return Strings.PolishDataGen.RegonValidationExtremeValuesMessage;
        if (CalculateControlNumber(value) != (value[value.Length == 9 ? KStartIndex : KStartIndexLong] - '0'))
            yield return Strings.PolishDataGen.RegonValidationControlNumberMessage;
    }
}
